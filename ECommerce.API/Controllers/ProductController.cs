using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetProducts() => Ok(await _repository.GetProductsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            
            if (product == default(Product))
                return BadRequest("The Product can not be find");

            return Ok(product);
        }
        
        [HttpGet("brands")]
        public async Task<IActionResult> GetProductBrands() => Ok(await _repository.GetProductBrandsAsync());

        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes() => Ok(await _repository.GetProductTypesAsync());
    }
}