using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingClub.Application.UseCases.Usuario.Register;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUsuarioJson),StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices]IRegisterUsuarioUseCase useCase,
            [FromBody]RequestRegisterUsuarioJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }
    }
}
