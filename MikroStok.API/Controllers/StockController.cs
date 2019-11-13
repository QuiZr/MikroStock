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
    public class StockController : Controller
    {
        private readonly ICommandsBus _commandBus;
        private readonly IQueryBus _queryBus;

        public StockController(ICommandsBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        [HttpPut]
        [Produces(typeof(Guid))]
        [Route("AddStockToWarehouse")]
        public async Task<IActionResult> AddStockToWarehouse(Guid warehouseId, string productName, int count)
        {
            var stocks = await _queryBus.Query<GetStockInWarehouseQuery, IReadOnlyList<Stock>>(
                new GetStockInWarehouseQuery{WarehouseId = warehouseId}
                );
            var stockId = stocks.SingleOrDefault(x => x.ProductName == productName)?.Id ?? Guid.NewGuid();
            return await AddStockToWarehouseWithKnownStockId(stockId, warehouseId, productName, count);
        }
        
        [HttpPut]
        [Produces(typeof(Guid))]
        [Route("AddStockToWarehouseWithKnownStockId")]
        public async Task<IActionResult> AddStockToWarehouseWithKnownStockId(Guid stockId, Guid warehouseId, string productName, int count)
        {
            var command = new AddStockToWarehouseCommand
            {
                Id = stockId,
                WarehouseId = warehouseId,
                ProductName = productName,
                Count = count
            };
            await _commandBus.Send(command);

            return Ok(stockId);
        }
        
        [HttpPut]
        [Produces(typeof(Guid))]
        [Route("WithdrawStockFromWarehouse")]
        public async Task<IActionResult> WithdrawStockFromWarehouse(Guid warehouseId, string productName, int count)
        {
            var stocks = await _queryBus.Query<GetStockInWarehouseQuery, IReadOnlyList<Stock>>(
                new GetStockInWarehouseQuery{WarehouseId = warehouseId}
            );
            var stockId = stocks.SingleOrDefault(x => x.ProductName == productName)?.Id ?? Guid.NewGuid();
            return await WithdrawStockFromWarehouseWithKnownStockId(stockId, count);
        }
        
        [HttpPut]
        [Produces(typeof(Guid))]
        [Route("WithdrawStockFromWarehouseWithKnownStockId")]
        public async Task<IActionResult> WithdrawStockFromWarehouseWithKnownStockId(Guid stockId, int count)
        {
            var command = new WithdrawStockFromWarehouseCommand()
            {
                Id = stockId,
                Count = count
            };
            await _commandBus.Send(command);

            return Ok(stockId);
        }
        
        [HttpGet]
        [Produces(typeof(IEnumerable<Stock>))]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllStockQuery();
            return Ok(await _queryBus.Query<GetAllStockQuery, IReadOnlyList<Stock>>(query));
        }
        
        [HttpGet]
        [Produces(typeof(IEnumerable<Stock>))]
        [Route("GetForWarehouse")]
        public async Task<IActionResult> GetForWarehouse(Guid warehouseId)
        {
            var query = new GetStockInWarehouseQuery
            {
                WarehouseId = warehouseId
            };
            return Ok(await _queryBus.Query<GetStockInWarehouseQuery, IReadOnlyList<Stock>>(query));
        }
    }
}