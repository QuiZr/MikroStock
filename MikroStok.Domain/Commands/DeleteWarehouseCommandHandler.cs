using System.Threading.Tasks;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Events;
using MikroStok.ES.Core.Events;

namespace MikroStok.Domain.Commands
{
    public class DeleteWarehouseCommandHandler : IHandleCommand<DeleteWarehouseCommand>
    {
        private readonly IEventBus _eventBus;

        public DeleteWarehouseCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task Handle(DeleteWarehouseCommand command)
        {
            var e = new WarehouseDeletedEvent(command.Id, command.Version);
            await _eventBus.Publish(e);
        }
    }
}