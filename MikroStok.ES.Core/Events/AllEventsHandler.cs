using System.Threading.Tasks;
using Marten;

namespace MikroStok.ES.Core.Events
{
    public class AllEventsHandler<TEvent> : IHandleEvent<TEvent> where TEvent : IEvent
    {
        private readonly IDocumentStore _documentStore;

        public AllEventsHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task Handle(TEvent e)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(e.Id, e.Version, e);
                await session.SaveChangesAsync();
            }
        }
    }
}