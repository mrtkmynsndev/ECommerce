using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Specifications;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductController(
            IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> productTypeRepository,
            IMapper mapper)
        {
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] ProductSpecParam productSpecParam)
        {

            var spec = new ProductsWithTypesAndBrandsSpecification(productSpecParam);

            var countSpec = new ProductWithFilterCountSpecification(productSpecParam);

            var products = await _productRepository.ListAsync(spec);

            var totalItems = await _productRepository.CountAsync(countSpec);

            var mapData = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            return OkWithPagination(productSpecParam.PageIndex, productSpecParam.PageSize, totalItems, mapData);

            //return Ok(new Pagination<ProductDto>(productSpecParam.PageIndex, productSpecParam.PageSize, totalItems, mapData));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpecAsync(spec);

            if (product == default(Product))
                return NotFound(new ApiResponse(404));


            return Ok(_mapper.Map<Product, ProductDto>(product));
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetProductBrands()
            => Ok(await _productBrandRepository.ListAllAsync());

        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes()
            => Ok(await _productTypeRepository.ListAllAsync());
    }
}