using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Arma.Register;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
 //[AuthenticatedUsuario]
    public class ArmaController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredArmaJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterArmaUseCase useCase,
            [FromBody] RequestArmaBaseJson request)
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }
    }
}
