using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity<int>;
         Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}