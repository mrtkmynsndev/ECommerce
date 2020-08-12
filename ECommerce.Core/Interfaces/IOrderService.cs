using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Core.Entities.Orders;

namespace ECommerce.Core.Interfaces
{
    public interface IOrderService
    {
         Task<Order> CreateOrderAsync(string userName, int deliveryMethodId, string basketId, Adress shippingAdress);

         Task<IReadOnlyList<Order>> GetOrdersByUserNameAsync(string userName);

         Task<Order> GetOrderByIdAsync(int id, string userName);

         Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}