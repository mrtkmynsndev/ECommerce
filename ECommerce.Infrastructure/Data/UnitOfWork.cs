using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;

namespace ECommerce.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositoryHash;
        private bool disposedValue;
        private readonly ECommerceContext _context;

        public UnitOfWork(ECommerceContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity<int>
        {
            if (_repositoryHash == default(Hashtable))
                _repositoryHash = new Hashtable();

            var entityName = typeof(T).Name;

            if (!_repositoryHash.ContainsKey(entityName))
            {
                var genericRepositoryType = typeof(GenericRepository<>);

                var genericRepositoryInstance = Activator.CreateInstance(genericRepositoryType.MakeGenericType(typeof(T)), _context);

                _repositoryHash.Add(entityName, genericRepositoryInstance);
            }
            return (IGenericRepository<T>)_repositoryHash[entityName];
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}