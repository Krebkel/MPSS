using Contracts;
using Contracts.ProjectEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProjectConfigurations;

internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Project)
            .WithMany()
            .IsRequired();
        builder.Property(e => e.Amount).IsRequired(false).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion(
                et => et.ToString(),
                s => (ExpenseType)Enum.Parse(typeof(ExpenseType), s)
            );
    }
}