using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Models;
using MikroStok.Domain.Queries;

namespace MikroStok.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : Controller
    {
        private readonly ICommandsBus _commandBus;
        private readonly IQueryBus _queryBus;
        
        public WarehouseController(ICommandsBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        [HttpPost]
        [Produces(typeof(Guid))]
        [Route("Create")]
        public async Task<IActionResult> Create(string name)
        {
            var command = new CreateWarehouseCommand
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            await _commandBus.Send(command);

            return Ok(command.Id);
        }
        
        [HttpDelete]
        [Produces(typeof(Guid))]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var warehouses = await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(new GetWarehousesQuery());
            var version = warehouses.Single(x => x.Id == id).Version;
            return await DeleteWithKnownVersion(id, version);
        }

        [HttpDelete]
        [Produces(typeof(Guid))]
        [Route("DeleteWithKnownVersion")]
        public async Task<IActionResult> DeleteWithKnownVersion(Guid id, int version)
        {
            var command = new DeleteWarehouseCommand(id, version);
            await _commandBus.Send(command);

            return Ok(command.Id);
        }
        
        [HttpGet]
        [Produces(typeof(IEnumerable<Warehouse>))]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _queryBus.Query<GetWarehousesQuery, IReadOnlyList<Warehouse>>(new GetWarehousesQuery()));
        }
    }
}