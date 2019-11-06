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
            return await _documentStore.QuerySession()
                .Query<Warehouse>()
                .ToListAsync();
        }
    }
}