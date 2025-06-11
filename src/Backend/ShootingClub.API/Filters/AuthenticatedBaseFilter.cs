using Microsoft.AspNetCore.Mvc.Filters;
using ShootingClub.Exceptions.ExceptionsBase;
using ShootingClub.Exceptions;

namespace ShootingClub.API.Filters
{
    public abstract class AuthenticatedBaseFilter
    {
        protected static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authorization = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorization))
            {
                throw new ShootingClubException(ResourceMessagesException.SEM_TOKEN);
            }

            return authorization["Bearer ".Length..].Trim();
        }
    }
}
