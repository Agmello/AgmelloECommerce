using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Commands;
using Modules.Users.Application.Queries;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken token)
        {
            if (Guid.Empty == id) return BadRequest("Invalid ID");
            var result = await m_mediator.Send(new DeleteUserCommand(id), token);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] LoginCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var token = await m_mediator.Send(command);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            //var userId = User.FindFirstValue(JwtRegisteredClaimNames.Email);
            var userId = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { userId });
        }

    }
}
