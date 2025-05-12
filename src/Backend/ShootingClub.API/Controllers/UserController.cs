using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public IActionResult Register(RequestRegisterUserJson request)
        {
            return Created();
        }
    }
}
