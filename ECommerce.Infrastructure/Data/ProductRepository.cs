using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceContext _context;
        public ProductRepository(ECommerceContext context)
        {
            _context = context;

        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync() => await _context.ProductBrands.ToListAsync();

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync() => await _context.ProductTypes.ToListAsync();

    }
}