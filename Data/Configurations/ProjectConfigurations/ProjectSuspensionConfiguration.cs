using Contracts.ProjectEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProjectConfigurations;

internal class ProjectSuspensionConfiguration : IEntityTypeConfiguration<ProjectSuspension>
{
    public void Configure(EntityTypeBuilder<ProjectSuspension> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.HasOne(ps => ps.Project)
            .WithMany()
            .IsRequired();
        builder.Property(ps => ps.DateSuspended).IsRequired();
    }
}