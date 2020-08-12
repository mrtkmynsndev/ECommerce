namespace ECommerce.Core.Entities.Orders
{
    // Snapshot Order and Product
    public class ProductItemOrdered // Owned of Order
    {
        public ProductItemOrdered()
        {
            // Dealing with EfCore
        }

        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

    }
}