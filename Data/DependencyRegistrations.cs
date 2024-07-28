using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Data;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresData(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((provider, opt) =>
        {
            var options = provider.GetRequiredService<IOptions<DataOptions>>().Value;

            opt.UseNpgsql(options.ConnectionString,
                builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", options.ServiceSchema));
        });

        return services;
    }
}