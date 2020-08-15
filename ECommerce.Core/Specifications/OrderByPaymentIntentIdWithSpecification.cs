using ECommerce.Core.Entities.Orders;

namespace ECommerce.Core.Specifications
{
    public class OrderByPaymentIntentIdWithSpecification : Specification<Order>
    {
        public OrderByPaymentIntentIdWithSpecification(string paymentIntentId)
        : base(x => x.PaymentIntentId == paymentIntentId)
        {
            this.AddInclude(x => x.OrderItems);
        }
    }
}