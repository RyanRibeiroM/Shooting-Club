using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Clube.Register;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class ClubeController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredClubeJson), StatusCodes.Status201Created)]
        [AuthenticatedAdmin]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterClubeUseCase useCase,
            [FromBody] RequestRegisterClubeJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }
    }
}
