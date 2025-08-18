using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatalogItemAsync(Guid id, CancellationToken token = default)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid ID");
            var item = await m_mediator.Send(new GetCatalogItemQuery(id), token);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCatalogItemAsync(CancellationToken token = default)
        {
            var item = await m_mediator.Send(new GetAllCatalogItemQuery(), token);
            if (item == null)
                return NotFound();
            return Ok(item);
        }


    }
}
