using Contracts.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProductConfigurations;

internal class ProjectProductConfiguration : IEntityTypeConfiguration<ProjectProduct>
{
    public void Configure(EntityTypeBuilder<ProjectProduct> builder)
    {
        builder.HasKey(pp => pp.Id);
        builder.HasOne(pp => pp.Project)
            .WithMany()
            .IsRequired();
        builder.HasOne(pp => pp.Product)
            .WithMany()
            .IsRequired();
        builder.Property(pp => pp.Quantity).IsRequired();
        builder.Property(pp => pp.Markup).IsRequired().HasColumnType("decimal(18,2)");
    }
}