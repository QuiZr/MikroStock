using System.Threading.Tasks;
using MikroStok.CQRS.Core.Commands.Interfaces;
using MikroStok.Domain.Aggregates;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Commands
{
    public class DeleteWarehouseCommandHandler : IHandleCommand<DeleteWarehouseCommand>
    {
        private readonly IAggregateRepository _aggregateRepository;

        public DeleteWarehouseCommandHandler(IAggregateRepository aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task Handle(DeleteWarehouseCommand command)
        {
            var aggregate = await _aggregateRepository.Load<WarehouseAggregate>(command.Id);
            aggregate.Delete(command);
            _aggregateRepository.Store(aggregate);
        }
    }
}