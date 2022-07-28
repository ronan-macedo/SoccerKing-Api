using Microsoft.AspNetCore.Mvc;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SoccerKing.Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<object> Login([FromBody] UserEntity user, [FromServices] ILoginService service)
        {
            if (!ModelState.IsValid || user == null)
                return BadRequest(ModelState);

            try
            {
                object result = await service.FindByLogin(user);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
