using System;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.Domain.Commands
{
    public class AddStockToWarehouseCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
    }
}