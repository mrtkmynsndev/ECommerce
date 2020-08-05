using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Maps
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products").HasKey(k => k.Id);

            builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(100);
            builder.Property(p => p.Description).HasColumnName("Description").IsRequired(true).HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).HasColumnName("PictureUrl").IsRequired(true);

            builder.HasOne(x => x.ProductBrand).WithMany().HasForeignKey(f => f.ProductBrandId);
            builder.HasOne(x => x.ProductType).WithMany().HasForeignKey(f => f.ProductTypeId);
        }
    }
}