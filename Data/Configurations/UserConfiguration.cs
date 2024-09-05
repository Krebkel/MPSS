using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Password).IsRequired();
        builder.HasOne(es => es.Employee)
            .WithMany()
            .IsRequired();
        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion(
                ur => ur.ToString(),
                s => (UserRole)Enum.Parse(typeof(UserRole), s)
            );
    }
}