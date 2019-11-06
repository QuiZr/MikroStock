using System;
using System.Threading.Tasks;

namespace MikroStok.ES.Core
{
    public interface IAggregateRepository
    {
        void Store(AggregateBase aggregate);
        Task<T> Load<T>(Guid id, int? version = null) where T : AggregateBase, new();
    }
}