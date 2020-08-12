using System;
using System.Linq.Expressions;
using ECommerce.Core.Entities.Orders;

namespace ECommerce.Core.Specifications
{
    public class OrderWithItemsAndOrederingSpecification : Specification<Order>
    {
        public OrderWithItemsAndOrederingSpecification()
        {
            this.AddInclude(x => x.OrderItems);
            this.AddInclude(x => x.DeliveryMethod);
        }

        public OrderWithItemsAndOrederingSpecification(string userName) 
        : base(x => x.BuyerUserName.ToLower() == userName.ToLower())
        {
            this.AddInclude(x => x.OrderItems);
            this.AddInclude(x => x.DeliveryMethod);
        }

        public OrderWithItemsAndOrederingSpecification(int id, string userName)
        : base(x => x.Id == id && x.BuyerUserName == userName)
        {
            this.AddInclude(x => x.OrderItems);
            this.AddInclude(x => x.DeliveryMethod);
        }
    }
}