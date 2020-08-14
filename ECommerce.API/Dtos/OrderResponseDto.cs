using System;
using System.Collections.Generic;
using ECommerce.Core.Entities.Orders;

namespace ECommerce.API.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string BuyerUserName { get; set; }
        public Adress ShipToAdress { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems {get; set;}
        public decimal SubTotal { get; set; }
        public decimal Total {get; set;}
        public string Status { get; set; }
    }
}