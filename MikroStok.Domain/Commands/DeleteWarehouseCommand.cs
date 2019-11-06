using System;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.Domain.Commands
{
    public class DeleteWarehouseCommand : ICommand
    {
        public DeleteWarehouseCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}