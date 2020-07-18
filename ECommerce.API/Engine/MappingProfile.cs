
using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Helpers;
using ECommerce.Core.Entities;

namespace ECommerce.API.Engine
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(m => m.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(m => m.ProductType, o => o.MapFrom(p => p.ProductType.Name))
                .ForMember(m => m.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}