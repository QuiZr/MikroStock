using System;
using System.Threading.Tasks;
using MikroStok.CQRS.Core.Queries.Interfaces;

namespace MikroStok.CQRS.Core.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly Func<(Type, Type), IHandleQuery> _handlersFactory;

        public QueryBus(Func<(Type, Type), IHandleQuery> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }

        public async Task<TResult> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var handler = (IHandleQuery<TQuery, TResult>) _handlersFactory((typeof(TQuery), typeof(TResult)));
            return await handler.Handle(query);
        }
    }
}