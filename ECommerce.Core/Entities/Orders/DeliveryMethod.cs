using System.Linq;

namespace ECommerce.Core.Entities.Orders
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public static readonly DeliveryMethod UPS1 = new DeliveryMethod(1, "UPS1", "1-2 Days", "Fastest delivery time", 10);
        public static readonly DeliveryMethod UPS2 = new DeliveryMethod(2, "UPS2", "2-5 Days", "Get it within 5 days", 5);
        public static readonly DeliveryMethod UPS3 = new DeliveryMethod(3, "UPS3", "5-10 Days", "Slower but cheap", 2);
        public static readonly DeliveryMethod[] AllDeliveries = { UPS1, UPS2, UPS3 };

        protected DeliveryMethod()
        {

        }

        public DeliveryMethod(int id, string name, string deliveryTime, string description, decimal price)
        : base(id)
        {
            Name = name;
            DeliveryTime = deliveryTime;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public static DeliveryMethod FromId(int id)
        {
            return AllDeliveries.SingleOrDefault(x => x.Id == id);
        }
    }
}