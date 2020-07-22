using ECommerce.Core.Entities;

namespace ECommerce.Core.Specifications
{
    public class ProductWithFilterCountSpecification : Specification<Product>
    {
        public ProductWithFilterCountSpecification(ProductSpecParam param) : base(x =>
          (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search)) &&
          (!param.BrandId.HasValue || x.ProductBrandId == param.BrandId) &&
          (!param.TypeId.HasValue || x.ProductTypeId == param.TypeId)
        )
        {

        }
    }
}