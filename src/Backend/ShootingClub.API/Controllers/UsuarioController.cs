using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingClub.API.Attributes;
using ShootingClub.Application.UseCases.Usuario.ChangeSenha;
using ShootingClub.Application.UseCases.Usuario.Profile;
using ShootingClub.Application.UseCases.Usuario.Register;
using ShootingClub.Application.UseCases.Usuario.Update;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    public class UsuarioController : ShootingClubBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUsuarioJson), StatusCodes.Status201Created)]
        // [AuthenticatedAdmin]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUsuarioUseCase useCase,
            [FromBody] RequestRegisterUsuarioJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }


        [HttpGet]
        [ProducesResponseType(typeof(ResponseUsuarioProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> GetUsuarioProfile([FromServices] IGetUsuarioProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateUsuarioUseCase useCase,
            [FromBody] RequestUpdateUsuarioJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUsuario]
        public async Task<IActionResult> changeSenha(
            [FromServices] IChangeSenhaUseCase usecase,
            [FromBody] RequestChangeSenhaJson request)
        {
            await usecase.Execute(request);
            return NoContent();
        }

    }
}
