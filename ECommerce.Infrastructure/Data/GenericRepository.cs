using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        private readonly ECommerceContext _context;

        private DbSet<T> entities;
        protected DbSet<T> Table => entities ?? (entities = _context.Set<T>());

        public GenericRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await AppySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await AppySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await AppySpecification(spec).CountAsync();
        }

        private IQueryable<T> AppySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(Table.AsQueryable(), spec);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            var entityEntry = _context.Entry(entity);

            if (entityEntry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }
        }
    }
}