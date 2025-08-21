using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Commands;
using Modules.Users.Application.Queries;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.API
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        IMediator m_mediator;
        public UserController(IMediator mediator)
        {
            m_mediator = mediator;
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken token)
        {
            if(string.IsNullOrEmpty(email)) return BadRequest("Invalid email");
            var user = await m_mediator.Send(new GetUserByEmailQuery(email), token);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken token)
        {
            if (Guid.Empty == id) return BadRequest("Invalid ID");
            var user = await m_mediator.Send(new GetUserByIdQuery(id), token);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromQuery] AddUserCommand command, CancellationToken token)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var id = await m_mediator.Send(command, token);
            if (id == Guid.Empty)
                return BadRequest("User registration failed");
            return CreatedAtAction(nameof(GetByEmail), new { id }, id);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken token)
        {
            if (Guid.Empty == id) return BadRequest("Invalid ID");
            var result = await m_mediator.Send(new DeleteUserCommand(id), token);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
