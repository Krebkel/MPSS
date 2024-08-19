using Contracts.ProjectEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.ProjectConfigurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Address).IsRequired();
            builder.Property(p => p.DeadlineDate).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.DateSuspended).IsRequired(false);
            builder.Property(p => p.CounteragentId).IsRequired(false);
            builder.Property(p => p.TotalCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.ResponsibleEmployeeId).IsRequired();
            builder.Property(p => p.ManagerShare).IsRequired();

            builder.Property(p => p.ProjectStatus)
                .IsRequired()
                .HasConversion<string>();
            
            builder.HasOne<Counteragent>()
                .WithMany()
                .HasForeignKey(p => p.CounteragentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}