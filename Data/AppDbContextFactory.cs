using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Data;

/// <summary>
/// Фабрика DbContext'а для создания миграций.
/// </summary>
internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var dataOptions = Options.Create(new DataOptions
        {
            ConnectionString =
                "Host=***;Port=5432;Database=***;Username=***;Password=***;Pooling=true;Maximum Pool Size=10",
            ServiceSchema = "mpss"
        });

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(dataOptions.Value.ConnectionString,
            builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", dataOptions.Value.ServiceSchema));

        return new AppDbContext(optionsBuilder.Options, dataOptions);
    }
}