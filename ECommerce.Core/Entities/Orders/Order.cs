using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Core.Entities.Orders
{
    public class Order : BaseEntity<int>
    {
        public Order()
        {

        }

        public Order(string buyerUserName, Adress shipToAdress,
        DeliveryMethod deliveryMethod, string paymentIntentId)
        {
            BuyerUserName = buyerUserName;
            ShipToAdress = shipToAdress;
            DeliveryMethod = deliveryMethod;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerUserName { get; set; }
        public Adress ShipToAdress { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public DeliveryMethod DeliveryMethod { get; set; }
        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems;
        public decimal SubTotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;

        public void AddOrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
            if (this._orderItems.Any(x => x.ItemOrdered.ProductName == itemOrdered.ProductName))
                throw new InvalidOperationException("The OrderItem is already exist!");

            OrderItem orderedItem = new OrderItem(itemOrdered, price, quantity);

            this._orderItems.Add(orderedItem);

            CalculateSubTotals();
        }

        public void ChangeStatus(OrderStatus status){
            this.Status = status;
        }

        private void CalculateSubTotals()
        {
            this.SubTotal = _orderItems.Sum(x => x.Price * x.Quantity);
        }
    }
}