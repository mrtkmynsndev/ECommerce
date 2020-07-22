using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Specifications;

namespace ECommerce.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity<int>
    {
         Task<T> GetByIdAsync(int id);
         Task<IReadOnlyList<T>> ListAllAsync();
         Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
         Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
         Task<int> CountAsync(ISpecification<T> spec); 
    }
}