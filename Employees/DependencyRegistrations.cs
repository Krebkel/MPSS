using Employees.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Employees;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresInvoices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeShiftService, EmployeeShiftService>();

        return services;
    }
}