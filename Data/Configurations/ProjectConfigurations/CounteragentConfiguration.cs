using Contracts.ProjectEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProjectConfigurations;

internal class CounteragentConfiguration : IEntityTypeConfiguration<Counteragent>
{
    public void Configure(EntityTypeBuilder<Counteragent> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Contact).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Phone).IsRequired().HasMaxLength(15);
        builder.Property(c => c.INN).IsRequired(false);
        builder.Property(c => c.OGRN).IsRequired(false);
        builder.Property(c => c.AccountNumber).IsRequired(false);
        builder.Property(c => c.BIK).IsRequired(false);
    }
}