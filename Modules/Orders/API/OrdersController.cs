using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Orders.Application.Commands;
using Modules.Orders.Application.Dtos;
using Modules.Orders.Application.Queries;
using Modules.Orders.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Orders.API
{
    [Controller]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private IMediator m_mediator;
        public OrdersController(IMediator mediator)
        {
            m_mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        private Guid GetUserGuid()
        {
            var userIdString = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var userId = new Guid(userIdString);
            return userId;
        }
        private bool IsMe(Guid id)
        {
            var userIdString = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var userId = Guid.Parse(userIdString);
            return userId ==  id;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersAsync(Guid id, CancellationToken token)
        {
            var userId = GetUserGuid();
            var result = await m_mediator.Send(new GetOrderQuery { OrderId = id }, token);

            if (result == null)
                return NotFound();
            if(result.UserId != userId)
                return Forbid();
            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUserAsync(Guid userId, CancellationToken token)
        {
            if(!IsMe(userId))
                Forbid();

            var items = await m_mediator.Send(new GetOrderForUserQuery(userId), token);
            if (items == null)
                return NotFound();
            return Ok(items);

        }

        [HttpGet("all")]
        [Authorize(Policy = "Adminstrators")]
        public async Task<IActionResult> GetAllOrdersAsync(CancellationToken token)
        {
            var result = await m_mediator.Send(new GetAllOrdersQuery(), token);

            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("orderitems/all")]
        [Authorize(Policy = "Adminstrators")]
        public async Task<IActionResult> GetAllOrdersItemsAsync(CancellationToken token)
        {
            var result = await m_mediator.Send(new GetAllOrderItemsQuery(), token);

            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync([FromBody] AddOrderDto order, CancellationToken token)
        {
            var userId = GetUserGuid();
            if(order.UserId != userId)
                return Forbid();

            var result = await m_mediator.Send(new AddOrderCommand(order), token);
            return Ok(result);
        }
        [HttpPatch]
        [Authorize(Policy = "Handlers")]
        public async Task<IActionResult> UpdateField(Guid id, string field, object? value)
        {
            await m_mediator.Send(new UpdateFieldCommand(id, field, value));
            return Ok();
        }
        [HttpDelete]
        [Authorize(Policy = "CanManageOrder")]
        public async Task<IActionResult> DeleteOrderAsync(Guid id, CancellationToken token)
        {
            return await m_mediator.Send(new DeleteOrderCommand(id), token) is bool result ?
                Ok(result) : NotFound();
            
        }
        [HttpDelete("purge")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> PurgeAsync(CancellationToken token)
        {
            return await m_mediator.Send(new PurgeOrderCommand(), token) is int count
                ? Ok(new { deleted = count })
                : BadRequest("Purge failed");
        }
        
    }
}
