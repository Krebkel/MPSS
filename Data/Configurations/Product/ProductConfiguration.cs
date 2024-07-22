using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.Product;

internal class ProductConfiguration : IEntityTypeConfiguration<Contracts.Product.Product>
{
    public void Configure(EntityTypeBuilder<Contracts.Product.Product> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(es => es.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Cost).IsRequired().HasColumnType("decimal(18,2)");
    }
}