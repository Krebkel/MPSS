using Contracts.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProductConfigurations;

internal class ProductComponentConfiguration : IEntityTypeConfiguration<ProductComponent>
{
    public void Configure(EntityTypeBuilder<ProductComponent> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.HasOne(pc => pc.Product)
            .WithMany()
            .IsRequired();
        builder.HasOne(pc => pc.Component)
            .WithMany()
            .IsRequired();
        builder.Property(pc => pc.Quantity).IsRequired(false);
    }
}