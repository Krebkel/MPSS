using Contracts.EmployeeEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.EmployeeConfigurations;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Phone).IsRequired().HasMaxLength(20);
        builder.Property(e => e.IsDriver).IsRequired();
        builder.Property(e => e.Passport).IsRequired(false).HasMaxLength(10);
        builder.Property(e => e.INN).IsRequired(false).HasMaxLength(12);
        builder.Property(e => e.AccountNumber).IsRequired(false).HasMaxLength(20);
        builder.Property(e => e.BIK).IsRequired(false).HasMaxLength(9);
    }
}