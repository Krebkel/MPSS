using Contracts.FileEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.FileConfigurations;

internal class ProjectFileConfiguration : IEntityTypeConfiguration<ProjectFile>
{
    public void Configure(EntityTypeBuilder<ProjectFile> builder)
    {
        builder.HasKey(pf => pf.Id);
        builder.Property(pf => pf.Name).IsRequired();
        builder.Property(pf => pf.FilePath).IsRequired();
        builder.Property(pf => pf.UploadDate).IsRequired();
        builder.HasOne(pf => pf.Project)
            .WithMany()
            .IsRequired(false);
    }
}