using Products.Services;
using Web.Requests.ProductRequests;

namespace Web.Extensions.ProductExtensions;

public static class ProductComponentExtension
{
    internal static CreateProductComponentRequest ToCreateProductComponentApiRequest(
        this CreateProductComponentApiRequest request)
    {
        return new CreateProductComponentRequest
        {
            Product = request.Product,
            Component = request.Component,
            Quantity = request.Quantity,
        };
    }
    
    internal static UpdateProductComponentRequest ToUpdateProductComponentApiRequest(
        this UpdateProductComponentApiRequest request)
    {
        return new UpdateProductComponentRequest
        {
            Id = request.Id,
            Product = request.Product,
            Component = request.Component,
            Quantity = request.Quantity,
        };
    }
}