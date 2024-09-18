using Files.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Files;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresFiles(this IServiceCollection services)
    {
        services.AddScoped<IProjectFileService, ProjectFileService>();

        return services;
    }
}