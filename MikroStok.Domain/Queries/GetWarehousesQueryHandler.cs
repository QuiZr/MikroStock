using System.Collections.Generic;
using System.Threading.Tasks;
using Marten;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Models;

namespace MikroStok.Domain.Queries
{
    public class GetWarehousesQueryHandler : IHandleQuery<GetWarehousesQuery, IReadOnlyList<Warehouse>>
    {
        private readonly IDocumentStore _documentStore;

        public GetWarehousesQueryHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<IReadOnlyList<Warehouse>> Handle(GetWarehousesQuery query)
        {
            using (var session = _documentStore.QuerySession())
            {
                return await session
                    .Query<Warehouse>()
                    .ToListAsync();
            }
        }
    }
}