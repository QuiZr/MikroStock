using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;

namespace MikroStok.ES.Core
{
    public sealed class AggregateRepository : IAggregateRepository
    {
        private readonly IDocumentStore _store;

        public AggregateRepository(IDocumentStore store)
        {
            _store = store;
        }

        public void Store(AggregateBase aggregate)
        {
            using (var session = _store.OpenSession())
            {
                // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
                var events = aggregate.GetUncommittedEvents().ToArray();
                session.Events.Append(aggregate.Id, aggregate.Version, events);
                session.SaveChanges();
            }

            // Once successfully persisted, clear events from list of uncommitted events
            aggregate.ClearUncommittedEvents();
        }

        public async Task<T> Load<T>(Guid id, int? version = null) where T : AggregateBase, new()
        {
            using (var session = _store.LightweightSession())
            {
                var aggregate = await session.Events.AggregateStreamAsync<T>(id, version ?? 0);
                return aggregate;
            }
        }
    }
}