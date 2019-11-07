using System;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.Domain.Commands
{
    public class WithdrawStockFromWarehouseCommand : ICommand
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public int Count { get; set; }
    }
}