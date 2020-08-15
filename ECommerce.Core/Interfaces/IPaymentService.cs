using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Orders;

namespace ECommerce.Core.Interfaces
{
    public interface IPaymentService
    {
         Task<CustomerBasket> CreateOrUpdatePaymentIntent(string id);
         Task<Order> UpdatePaymentIntentSucceeded(string paymentIntentId);
         Task<Order> UpdatePaymentIntentFailed(string paymentIntentId);
    }
}