using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.Employee;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Contracts.Employee.Employee>
{
    public void Configure(EntityTypeBuilder<Contracts.Employee.Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Phone).IsRequired().HasMaxLength(20);
        builder.Property(e => e.IsDriver).IsRequired();
        builder.Property(e => e.Passport).IsRequired(false);
        builder.Property(e => e.INN).IsRequired(false);
        builder.Property(e => e.AccountNumber).IsRequired(false);
        builder.Property(e => e.BIK).IsRequired(false);
    }
}