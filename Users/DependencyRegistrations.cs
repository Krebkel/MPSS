using Employees.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Users;

public static class DependencyRegistrations
{
    /// <summary>
    /// Зарегистрировать DbContext
    /// </summary>
    public static IServiceCollection AddUsers(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        
        return services;
    }
}