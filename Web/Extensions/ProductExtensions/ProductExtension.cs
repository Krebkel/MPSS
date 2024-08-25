using Products.Services;
using Web.Requests.ProductRequests;

namespace Web.Extensions.ProductExtensions;

public static class ProductExtension
{
    internal static CreateProductRequest ToCreateProductApiRequest(this CreateProductApiRequest request)
    {
        return new CreateProductRequest
        {
            Name = request.Name,
            Cost = request.Cost,
            Type = request.Type
        };
    }
    
    internal static UpdateProductRequest ToUpdateProductApiRequest(this UpdateProductApiRequest request)
    {
        return new UpdateProductRequest
        {
            Id = request.Id,
            Name = request.Name,
            Cost = request.Cost,
            Type = request.Type
        };
    }
}