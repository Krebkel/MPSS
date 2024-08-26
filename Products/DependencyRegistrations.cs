using Microsoft.Extensions.DependencyInjection;
using Products.Services;

namespace Products;

public static class DependencyRegistrations
{
    public static IServiceCollection AddPostgresProducts(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductComponentService, ProductComponentService>();
        services.AddScoped<IProjectProductService, ProjectProductService>();

        return services;
    }
}