using System;
using MikroStok.CQRS.Core.Commands.Interfaces;

namespace MikroStok.Domain.Commands
{
    public class DeleteWarehouseCommand : ICommand
    {
        public DeleteWarehouseCommand(Guid id, int version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}