using AutoMapper;
using FluentValidation.Results;
using ShootingClub.Application.UseCases.Arma.Validators;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Enums;
using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Arma.Update
{
    public class UpdateArmaUseCase : IUpdateArmaUseCase
    {

        private readonly IArmaUpdateOnlyRepository _updateOnlyRepository;
        private readonly IArmaReadOnlyRepository _armaReadOnlyRepository;
        private readonly IUsuarioReadOnlyRepository _usuarioRepository;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UpdateArmaUseCase(IArmaUpdateOnlyRepository updateOnlyRepository, IArmaReadOnlyRepository armaReadOnlyRepository, IUsuarioReadOnlyRepository usuarioRepository, ILoggedUsuario loggedUsuario, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _updateOnlyRepository = updateOnlyRepository;
            _armaReadOnlyRepository = armaReadOnlyRepository;
            _loggedUsuario = loggedUsuario;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }
        public async Task Execute(int ArmaId, RequestArmaBaseJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            await Validate(request, loggedUsuario.CPF, loggedUsuario.ClubeId, ArmaId);

            var arma = await _updateOnlyRepository.GetById(loggedUsuario, ArmaId);

            if (arma is null)
                throw new NotFoundException(ResourceMessagesException.ARMA_NOT_FOUND);

            if ((int)arma.TipoPosse == (int)request.TipoPosse)
            {
                _mapper.Map(request, arma);
            }
            else
            {
                HandleTypeChange(request, arma);
            }

            if (!string.IsNullOrEmpty(request.Cpf_proprietario))
            {
                var cpf_proprietario = CpfUtils.Format(request.Cpf_proprietario);
                int id_proprietario = await _usuarioRepository.GetIdUsuarioByCPF(cpf_proprietario);
                arma.UsuarioId = id_proprietario;
                arma.ClubeId = 0;
            }
            else
            {
                arma.ClubeId = loggedUsuario.ClubeId;
                arma.UsuarioId = 0;
            }

            arma.AtualizadoEm = DateTime.UtcNow;
            arma.NumeroSerie = arma.NumeroSerie.ToUpperInvariant();

            _updateOnlyRepository.Update(arma);
            await _unitOfWork.Commit();
        }


        private void HandleTypeChange(RequestArmaBaseJson request, ArmaBase arma)
        {
            switch (arma)
            {
                case ArmaExercito armaExercito:
                    armaExercito.NumeroSigma = null;
                    armaExercito.LocalRegistro = null;
                    armaExercito.DataExpedicaoCRAF = null;
                    armaExercito.ValidadeCRAF = null;
                    armaExercito.NumeroGTE = null;
                    armaExercito.ValidadeGTE = null;
                    break;
                case ArmaPF armaPF:
                    armaPF.DataValidadePF = null;
                    armaPF.NumeroSinarm = null;
                    armaPF.NumeroNotaFiscal = null;
                    break;
                case ArmaPortePessoal armaPorte:
                    armaPorte.ValidadeCertificacao = null;
                    armaPorte.NumeroCertificado = null;
                    break;
            }

            _mapper.Map(request, arma);

            arma.TipoPosse = (Domain.Enums.TipoPosseArma)request.TipoPosse;
        }

        private async Task Validate(RequestArmaBaseJson request, string cpf_loggedUsuario, int clubeId_loggedUsuario, int armaId)
        {
            ValidationResult? result = null;
            switch (request.TipoPosse)
            {
                case TipoPosseArma.Exercito:
                    var validatorExercito = new ArmaExercitoValidator();
                    result = await validatorExercito.ValidateAsync((RequestArmaExercitoJson)request);
                    break;

                case TipoPosseArma.PoliciaFederal:
                    var validatorPF = new ArmaPFValidator();
                    result = await validatorPF.ValidateAsync((RequestArmaPFJson)request);
                    break;

                case TipoPosseArma.PortePessoal:
                    var validatorPorte = new ArmaPorteValidator();
                    result = await validatorPorte.ValidateAsync((RequestArmaPorteJson)request);
                    break;

                default:
                    throw new ErrorOnValidationException([ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO]);
            }

            bool existNumeroSerie = await _updateOnlyRepository.ExistActiveArmaWithNumeroSerieAndIgnoreId(request.NumeroSerie.ToUpperInvariant(), armaId);
            if (existNumeroSerie)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.NUMERO_SERIE_JA_REGISTRADO));
            }
            if (!string.IsNullOrEmpty(request.Cpf_proprietario) && CpfUtils.ValidCPF(request.Cpf_proprietario))
            {
                var cpf_proprietario = CpfUtils.Format(request.Cpf_proprietario);
                bool existClubeAndCPF = await _usuarioRepository.ExistActiveUsuarioWithClubeAndCPF(clubeId_loggedUsuario, cpf_proprietario);

                if (!existClubeAndCPF)
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.CPF_USUARIO_INEXISTENTE));
                if (cpf_loggedUsuario.Equals(request.Cpf_proprietario))
                    result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.ACAO_INVALIDA_CPF_INFORMADO));
            }

            if (result is not null && !result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
