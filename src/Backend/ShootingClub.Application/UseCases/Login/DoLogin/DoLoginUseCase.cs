using ShootingClub.Application.Services.Cryptography;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {

        private readonly IUsuarioReadOnlyRepository _repository;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public DoLoginUseCase(IUsuarioReadOnlyRepository repository, PasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _accessTokenGenerator = accessTokenGenerator;

        }
        public async Task<ResponseLoggedInUsuarioJson> Execute(RequestLoginJson request)
        {
            var encriptedSenha = _passwordEncripter.Encrypt(request.Senha);
            var usuario = await _repository.GetByEmailAndSenha(request.Email, encriptedSenha) ?? throw new InvalidLoginException();


            return new ResponseLoggedInUsuarioJson
            {
                Nome = usuario.Nome,
                Email = request.Email,
                Tokens = new ResponseTokensJson 
                { 
                    AccessToken = _accessTokenGenerator.Generate(usuario.IdentificadorUsuario, (int)usuario.Nivel)
                }
            };

        }
    }
}
