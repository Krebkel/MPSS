using Contracts.EmployeeEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.EmployeeConfigurations;

internal class EmployeeShiftConfiguration : IEntityTypeConfiguration<EmployeeShift>
{
    public void Configure(EntityTypeBuilder<EmployeeShift> builder)
    {
        builder.HasKey(es => es.Id);
        builder.Property(es => es.ProjectId).IsRequired();
        builder.Property(es => es.EmployeeId).IsRequired();
        builder.Property(es => es.Date).IsRequired();
        builder.Property(es => es.Arrival).IsRequired(false);
        builder.Property(es => es.Departure).IsRequired(false);
        builder.Property(es => es.HoursWorked).IsRequired(false).HasColumnType("float");
        builder.Property(es => es.TravelTime).IsRequired(false).HasColumnType("float");
        builder.Property(es => es.ConsiderTravel).IsRequired();
    }
}