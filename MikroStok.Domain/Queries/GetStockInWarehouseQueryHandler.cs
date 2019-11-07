using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using MikroStok.CQRS.Core.Queries.Interfaces;
using MikroStok.Domain.Models;

namespace MikroStok.Domain.Queries
{
    public class GetStockInWarehouseQueryHandler : IHandleQuery<GetStockInWarehouseQuery, IReadOnlyList<Stock>>
    {       
        private readonly IDocumentStore _documentStore;

        public GetStockInWarehouseQueryHandler(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }
        
        public async Task<IReadOnlyList<Stock>> Handle(GetStockInWarehouseQuery query)
        {
            using (var session = _documentStore.QuerySession())
            {
                return await session
                    .Query<Stock>()
                    .Where(x => x.WarehouseId == query.WarehouseId)
                    .ToListAsync();
            }
        }
    }
}