using System.Collections.Generic;
using System.Threading.Tasks;
using Marten;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Models;

namespace MikroStok.Domain.Queries
{
    public class GetAllStockQueryHandler : IHandleQuery<GetAllStockQuery, IReadOnlyList<Stock>>
    {
        private readonly IDocumentStore _documentStore;

        public GetAllStockQueryHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<IReadOnlyList<Stock>> Handle(GetAllStockQuery query)
        {
            using (var session = _documentStore.QuerySession())
            {
                return await session
                    .Query<Stock>()
                    .ToListAsync();
            }
        }
    }
}