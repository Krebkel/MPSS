using Contracts.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.Project;

internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ProjectId).IsRequired();
        builder.Property(e => e.Amount).IsRequired(false).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(e => e.Type).IsRequired();
    }
}