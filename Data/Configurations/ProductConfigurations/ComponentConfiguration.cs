using Contracts.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProductConfigurations;

internal class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Weight).IsRequired(false);
        builder.Property(c => c.Price).IsRequired(false);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
    }
}