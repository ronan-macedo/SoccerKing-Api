using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerKing.Api.Domain.Dtos.User;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces.Services.User;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SoccerKing.Api.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: api/<UsersController>        
        [HttpGet, Authorize("Bearer")]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET api/<UsersController>/{id}        
        [HttpGet("{id}", Name = "GetWithId"), Authorize("Bearer")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _service.Get(id));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // POST api/<UsersController>        
        [HttpPost, Authorize("Bearer")]
        public async Task<ActionResult> Post([FromBody] UserDtoCreate user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                UserDtoCreateResult result = await _service.Post(user);

                if (result == null)
                    return BadRequest();

                return Created(new Uri(Url.Link("GetWithId", new { id = result.Id })), result);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // PUT api/<UsersController>/{id}        
        [HttpPut, Authorize("Bearer")]
        public async Task<ActionResult> Put([FromBody] UserDtoUpdate user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                UserDtoUpdateResult result = await _service.Put(user);

                if (result == null)
                    return BadRequest();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // DELETE api/<UsersController>/{id}        
        [HttpDelete("{id}"), Authorize("Bearer")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _service.Delete(id));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
