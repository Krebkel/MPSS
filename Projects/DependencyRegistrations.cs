using Microsoft.Extensions.DependencyInjection;
using Projects.Services;

namespace Projects;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresProjects(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICounteragentService, CounteragentService>();
        services.AddScoped<IExpenseService, ExpenseService>();

        return services;
    }
}