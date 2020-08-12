
using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Helpers;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Identity;
using ECommerce.Core.Entities.Orders;

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

            CreateMap<ECommerce.Core.Entities.Identity.Adress, AdressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AdressDto, ECommerce.Core.Entities.Orders.Adress>();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(x => x.DeliveryMethod, o => o.MapFrom(p => p.DeliveryMethod.Name))
                .ForMember(x => x.ShippingPrice, o => o.MapFrom(p => p.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductId, o => o.MapFrom(p => p.ItemOrdered.ProductItemId))
                .ForMember(x => x.ProductName, o => o.MapFrom(p => p.ItemOrdered.ProductName))
                .ForMember(x => x.PictureUrl, o => o.MapFrom(p => p.ItemOrdered.PictureUrl))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>())
                .ForMember(x => x.Price, o => o.MapFrom(p => p.Price))
                .ForMember(x => x.Quantity, o => o.MapFrom(p => p.Quantity));
        }
    }
}