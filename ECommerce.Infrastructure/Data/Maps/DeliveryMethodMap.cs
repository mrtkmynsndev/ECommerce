using ECommerce.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Maps
{
    public class DeliveryMethodMap : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name);
            builder.Property(x => x.Description);
            builder.Property(x => x.DeliveryTime);
            builder.Property(x => x.Price).HasColumnType("decimal(18, 2)");
        }
    }
}