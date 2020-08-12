using System;
using ECommerce.Core.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Maps
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.ShipToAdress, options =>
            {
                options.WithOwner();
            });

            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );

            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade) // Delete all related items
            .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field); 
        }
    }
}