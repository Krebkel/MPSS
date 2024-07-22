using Contracts.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.Product;

internal class ProductComponentConfiguration : IEntityTypeConfiguration<ProductComponent>
{
    public void Configure(EntityTypeBuilder<ProductComponent> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(pc => pc.ProductId).IsRequired();
        builder.Property(pc => pc.Name).IsRequired().HasMaxLength(100);
        builder.Property(pc => pc.Quantity).IsRequired(false);
        builder.Property(pc => pc.Weight).IsRequired(false);
    }
}