using Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Products;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresInvoices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductComponentService, ProductComponentService>();
        services.AddScoped<IProjectProductService, ProjectProductService>();

        return services;
    }
}