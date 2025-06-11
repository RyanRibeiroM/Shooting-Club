
using AutoMapper;
using ShootingClub.Application.Services.Cryptography;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Usuario.Register
{
    public class RegisterUsuarioUseCase : IRegisterUsuarioUseCase
    {
        private readonly IUsuarioReadOnlyRepository _usuarioReadOnlyRepository;
        private readonly IUsuarioWriteOnlyRepository _usuarioWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public RegisterUsuarioUseCase(
            IUsuarioReadOnlyRepository usuarioReadOnlyRepository,
            IUsuarioWriteOnlyRepository usuarioWriteOnlyRepository,
            IMapper mapper,
            PasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork,
            IAccessTokenGenerator accessTokenGenerator
            )
        {
            _usuarioReadOnlyRepository = usuarioReadOnlyRepository;
            _usuarioWriteOnlyRepository = usuarioWriteOnlyRepository;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<ResponseRegisteredUsuarioJson> Execute(RequestRegisterUsuarioJson request)
        {
            await Validate(request);

            var usuario = _mapper.Map<Domain.Entities.Usuario>(request);

            usuario.Senha = _passwordEncripter.Encrypt(request.Senha);
            usuario.IdentificadorUsuario = Guid.NewGuid();

            usuario.CPF = CpfFormatter.Format(request.CPF);
            usuario.AtualizadoEm = DateTime.Now;

            // salvar no banco de dados
            await _usuarioWriteOnlyRepository.Add(usuario);
            await _unitOfWork.Commit();

            return new ResponseRegisteredUsuarioJson { 
                Nome = request.Nome
            };

        }

        private async Task Validate(RequestRegisterUsuarioJson request)
        {
            var validator = new RegisterUsuarioValidator();

            var result = validator.Validate(request);

            var EmailExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithEmail(request.Email);
            var CPFexist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithCPF(request.CPF);
            var NumeroFiliacaoExist = await _usuarioReadOnlyRepository.ExistActiveUsuarioWithNumeroFiliacao(request.NumeroFiliacao);

            if (EmailExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_JA_CADASTRADO));

            if (CPFexist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.CPF_JA_CADASTRADO));

            if (NumeroFiliacaoExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.NUMERO_FILIACAO_JA_CADASTRADO));

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
