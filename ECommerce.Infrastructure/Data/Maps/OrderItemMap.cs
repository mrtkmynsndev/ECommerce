using ECommerce.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Maps
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.ItemOrdered, options => {
                options.WithOwner();
            });

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Quantity);
        }
    }
}