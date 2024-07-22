using Contracts.ProductEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProductConfigurations;

internal class ProjectProductConfiguration : IEntityTypeConfiguration<ProjectProduct>
{
    public void Configure(EntityTypeBuilder<ProjectProduct> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(es => es.ProjectId).IsRequired();
        builder.Property(m => m.ProductId).IsRequired();
        builder.Property(pc => pc.Quantity).IsRequired();
        builder.Property(m => m.Markup).IsRequired().HasColumnType("decimal(18,2)");
    }
}