using MikroStok.Domain.Commands;
using MikroStok.Domain.Events;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Aggregates
{
    public class WarehouseAggregate : AggregateBase
    {
        public WarehouseAggregate()
        {
        }

        public WarehouseAggregate(CreateWarehouseCommand command)
        {
            // Instantiation creates our initial event, capturing the invoice number
            var e = new WarehouseCreatedEvent(command.Id, command.Name);

            // Call Apply to mutate state of aggregate based on event
            Apply(e);

            // Add the event to uncommitted events to use it while persisting the events to Marten events store
            AddUncommittedEvent(e);
        }

        public string Name { get; set; }

        public void Delete(DeleteWarehouseCommand command)
        {
            var e = new WarehouseDeletedEvent(command.Id);
            Apply(e);
            AddUncommittedEvent(e);
        }

        private void Apply(WarehouseCreatedEvent e)
        {
            Id = e.Id;
            Name = e.Name;
            IsDeleted = false;
            // Ensure to update version on every Apply method.
            // There's probably a better way to do that.
            Version++;
        }

        private void Apply(WarehouseDeletedEvent e)
        {
            IsDeleted = true;
            // Ensure to update version on every Apply method.
            // There's probably a better way to do that.
            Version++;
        }
    }
}