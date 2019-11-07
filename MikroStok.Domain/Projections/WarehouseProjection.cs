using System;
using Marten;
using Marten.Events.Projections;
using MikroStok.Domain.Events;
using MikroStok.Domain.Models;

namespace MikroStok.Domain.Projections
{
    public class WarehouseProjection : ViewProjection<Warehouse, Guid>
    {
        public WarehouseProjection()
        {
            ProjectEvent<WarehouseCreatedEvent>(e => e.Id, Apply);
            DeleteEvent<WarehouseDeletedEvent>(e => e.Id);
        }

        private void Apply(IDocumentSession documentSession, Warehouse view, WarehouseCreatedEvent e)
        {
            view.Id = e.Id;
            view.Name = e.Name;
            view.Version++;
        }
    }
    
    public class StockProjection : ViewProjection<Stock, Guid>
    {
        public StockProjection()
        {
            ProjectEvent<StockAddedEvent>(e => e.Id, Apply);
            ProjectEvent<StockWithdrawnEvent>(e => e.Id, Apply);
        }

        private void Apply(IDocumentSession documentSession, Stock view, StockAddedEvent e)
        {
            view.Id = e.Id;
            view.ProductName = e.ProductName;
            view.Count += e.Count;
            view.WarehouseId = e.WarehouseId;
            view.Version++;
        }
        
        private void Apply(IDocumentSession documentSession, Stock view, StockWithdrawnEvent e)
        {
            view.Count -= e.Count;
            view.Version++;
        }
    }
}