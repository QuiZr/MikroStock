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
}