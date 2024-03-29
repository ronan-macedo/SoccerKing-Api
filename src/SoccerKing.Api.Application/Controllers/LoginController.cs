﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerKing.Api.Domain.Dtos;
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
        [HttpPost, AllowAnonymous]
        public async Task<object> Login([FromBody] LoginDto login, [FromServices] ILoginService service)
        {
            if (!ModelState.IsValid || login == null)
                return BadRequest(ModelState);

            try
            {
                object result = await service.FindByLogin(login);

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
