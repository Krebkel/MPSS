using Projects.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Projects;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresInvoices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICounteragentService, CounteragentService>();

        return services;
    }
}