using Contracts;
using Contracts.ProjectEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProjectConfigurations;

internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Address).IsRequired();
        builder.Property(p => p.DeadlineDate).IsRequired();
        builder.Property(p => p.StartDate).IsRequired();
        builder.HasOne(p => p.Counteragent)
            .WithMany()
            .IsRequired(false);
        builder.HasOne(p => p.ResponsibleEmployee)
            .WithMany()
            .IsRequired();        
        builder.Property(p => p.ManagerShare).IsRequired();
        builder.Property(p => p.ProjectStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ProjectStatus)Enum.Parse(typeof(ProjectStatus), v)
            );
    }
}