using FluentValidation;
using MikroStok.Domain.Commands;
using MikroStok.Domain.Events;
using MikroStok.ES.Core;

namespace MikroStok.Domain.Aggregates
{
    public class StockAggregate : AggregateBase
    {
        public StockAggregate()
        {
        }

        public int Count { get; set; }

        public void Add(AddStockToWarehouseCommand command)
        {
            var e = new StockAddedEvent(command.Id, command.WarehouseId, command.ProductName, command.Count, Version);
            Apply(e);
            AddUncommittedEvent(e);
        }
        
        public void Withdraw(WithdrawStockFromWarehouseCommand command)
        {
            if (command.Count < Count)
            {
                throw new ValidationException("Stock has less stock than required");
            }
            
            var e = new StockWithdrawnEvent(command.Id, command.Version, command.Count);
            Apply(e);
            AddUncommittedEvent(e);
        }
        
        private void Apply(StockAddedEvent e)
        {
            if (Version == 0)
            {
                Id = e.Id;
            }

            Count += Count;
            Version++;
        }
        
        private void Apply(StockWithdrawnEvent e)
        {
            Count -= Count;
            Version++;
        }
        
    }
}