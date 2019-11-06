using System;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.Domain.Commands
{
    public class CreateWarehouseCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}