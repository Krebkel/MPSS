using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProductConfigurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Contracts.ProductEntities.Product>
{
    public void Configure(EntityTypeBuilder<Contracts.ProductEntities.Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Cost).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion(
                pt => pt.ToString(),
                s => (ProductType)Enum.Parse(typeof(ProductType), s)
            );
    }
}