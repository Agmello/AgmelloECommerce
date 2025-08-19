using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.Catalog.Application;
using Modules.Catalog.Application.Commands;
using Modules.Catalog.Application.Queries;

namespace Modules.Catalog.API
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator m_mediator;

        public CatalogController(IMediator mediator)
        {
            m_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }




        [HttpDelete("purge")]
        public async Task<IActionResult> PurgeAsync(CancellationToken token = default)
        {
            var count = await m_mediator.Send(new PurgeCatalogItemTableCommand(), token);
            return Ok(new { deleted = count });
        }

        [HttpPost]
        public async Task<IActionResult> AddCatalogItemAsync(string name, decimal price, string description, string availability, string imageUrl, CancellationToken token = default)
        {
            var command = new CreateCatalogItemCommand(name, description, availability, price, imageUrl);
            var id = await m_mediator.Send(command, token);
            return CreatedAtAction(nameof(GetCatalogItem), new { id }, command.Item);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCatalogItemAsync(CancellationToken token = default)
        {
            var item = await m_mediator.Send(new GetAllCatalogItemQuery(), token);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatalogItem(Guid id, CancellationToken token = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid ID");
            var item = await m_mediator.Send(new GetCatalogItemQuery(id), token);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetCatalogItemWithFilter([FromQuery] CatalogFilterDto? filter, CancellationToken token = default)
        {
            if (filter is null)
                return BadRequest("Invalid filter");
            var item = await m_mediator.Send(new GetCatalogItemWithFilterQuery(filter), token);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
    }
}
