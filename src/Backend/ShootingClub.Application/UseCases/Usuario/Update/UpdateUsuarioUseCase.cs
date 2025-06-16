using AutoMapper;
using Microsoft.Extensions.Options;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Usuario.Update
{
    public class UpdateUsuarioUseCase : IUpdateUsuarioUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUsuarioUpdateOnlyRepository _repository;
        private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUsuarioUseCase(ILoggedUsuario loggedUsuario, IUsuarioUpdateOnlyRepository repository, IUsuarioReadOnlyRepository usuarioReadOnlyRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _loggedUsuario = loggedUsuario;
            _repository = repository;
            _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateUsuarioJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();
            await Validate(request, loggedUsuario.Email, loggedUsuario.CPF, loggedUsuario.DataFiliacao);

            var usuario = await _repository.GetById(loggedUsuario.Id);

            usuario.AtualizadoEm = DateTime.UtcNow;
            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.CPF = CpfUtils.Format(request.CPF);
            usuario.DataNascimento = request.DataNascimento;
            usuario.EnderecoPais = request.EnderecoPais;
            usuario.EnderecoEstado = request.EnderecoEstado;
            usuario.EnderecoCidade = request.EnderecoCidade;
            usuario.EnderecoBairro= request.EnderecoBairro;
            usuario.EnderecoRua = request.EnderecoRua;
            usuario.EnderecoNumero = request.EnderecoNumero;
            usuario.CR = request.CR;
            usuario.DataVencimentoCR = request.DataVencimentoCR;
            usuario.SFPCVinculacao = request.SFPCVinculacao;


            _repository.Update(usuario);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUsuarioJson request, string CurrentEmail, string CurrentCPF, DateOnly CurrentDataFiliacao)
        {
            var validator = new UpdateUsuarioValidator();

            var result = validator.Validate(request);

            if (!CurrentEmail.Equals(request.Email))
            {
                var usuarioExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithEmail(request.Email);
                if (usuarioExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_JA_CADASTRADO));
            }
            
            if (!CurrentCPF.Equals(request.CPF))
            {
                var usuarioExistCPF = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCPF(request.CPF);
                if (usuarioExistCPF)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("cpf", ResourceMessagesException.CPF_JA_CADASTRADO));
            }
        }
    }
}
