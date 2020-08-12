namespace ECommerce.API.Dtos
{
    public class OrderDto
    {
        public string UserName { get; set; }
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AdressDto ShippToAdress { get; set; }
    }
}