using AutoMapper;
using FluentValidation;
using ShootingClub.Application.UseCases.Arma.Validators;
using ShootingClub.Communication.Enums;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;
using System.Threading.Tasks;

namespace ShootingClub.Application.UseCases.Arma.Register
{
    public class RegisterArmaUseCase : IRegisterArmaUseCase
    {
        private readonly IArmaWriteOnlyRepository _repository;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterArmaUseCase(IArmaWriteOnlyRepository repository, ILoggedUsuario loggedUsuario, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _loggedUsuario = loggedUsuario;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisteredArmaJson> Execute(RequestArmaBaseJson request)
        {
            await Validate(request);

            var loggedUsuario = await _loggedUsuario.Usuario();

            var arma = _mapper.Map<ArmaBase>(request);

            arma.UsuarioId = loggedUsuario.Id;
            arma.ClubeId = loggedUsuario.ClubeId;

            await _repository.Add(arma);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredArmaJson>(arma);
        }

        private async Task Validate(RequestArmaBaseJson request)
        {
            // A lógica do switch deve ser exatamente esta:
            switch (request.TipoPosse)
            {
                // CASO EXÉRCITO
                case TipoPosseArma.Exercito:
                    var validatorExercito = new ArmaExercitoValidator();
                    var resultExercito = await validatorExercito.ValidateAsync((RequestArmaExercitoJson)request); // Cast para Exercito
                    if (!resultExercito.IsValid)
                        throw new ErrorOnValidationException(resultExercito.Errors.Select(e => e.ErrorMessage).ToList());
                    break;

                // CASO POLÍCIA FEDERAL
                case TipoPosseArma.PoliciaFederal:
                    var validatorPF = new ArmaPFValidator(); // Validador da PF
                    var resultPF = await validatorPF.ValidateAsync((RequestArmaPFJson)request); // Cast para PF
                    if (!resultPF.IsValid)
                        throw new ErrorOnValidationException(resultPF.Errors.Select(e => e.ErrorMessage).ToList());
                    break;

                // CASO PORTE PESSOAL
                case TipoPosseArma.PortePessoal:
                    var validatorPorte = new ArmaPorteValidator(); // Validador de Porte
                    var resultPorte = await validatorPorte.ValidateAsync((RequestArmaPorteJson)request); // Cast para Porte
                    if (!resultPorte.IsValid)
                        throw new ErrorOnValidationException(resultPorte.Errors.Select(e => e.ErrorMessage).ToList());
                    break;

                default:
                    throw new ErrorOnValidationException([ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO]);
            }
        }
    }
}