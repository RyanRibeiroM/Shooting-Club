using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Exceptions.ExceptionsBase;
using ShootingClub.Exceptions;

namespace ShootingClub.API.Filters
{
    public class AuthenticatedAdminFilter : AuthenticatedBaseFilter, IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUsuarioReadOnlyRepository _repository;

        public AuthenticatedAdminFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);

                var (identificador, _) = _accessTokenValidator.ValidateAndGetIdentificadorUsuarioAndNivel(token);
                var identificadorUsuario = identificador;
                var exist = await _repository.ExistActiveAdminWithIdentificador(identificadorUsuario);

                if (!exist)
                {
                    throw new ShootingClubException(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
                {
                    TokenIsExpired = true,
                });
            }
            catch (ShootingClubException ex)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USUARIO_SEM_PERMISSAO_PARA_ACESSAR_RECURSO));
            }

        }
    }
}
