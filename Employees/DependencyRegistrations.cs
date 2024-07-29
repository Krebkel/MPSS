using Employees.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employees;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresEmployees(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeShiftService, EmployeeShiftService>();

        return services;
    }
}