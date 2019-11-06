using System;
using MikroStok.CQRS.Core.Queries.Interfaces;

namespace MikroStok.CQRS.Core.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly Func<Type, IHandleQuery> _handlersFactory;

        public QueryBus(Func<Type, IHandleQuery> handlersFactory)

        {
            _handlersFactory = handlersFactory;
        }

        public TResult Send<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var handler = (IHandleQuery<TQuery, TResult>) _handlersFactory(typeof(TQuery));
            return handler.Handle(query);
        }
    }
}