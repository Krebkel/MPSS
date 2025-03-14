using Products.Services;
using Web.Requests.ProductRequests;

namespace Web.Extensions.ProductExtensions;

public static class ComponentExtension
{
    internal static CreateComponentRequest ToCreateComponentRequest(
        this CreateComponentApiRequest request)
    {
        return new CreateComponentRequest
        {
            Name = request.Name,
            Price = request.Price,
            Weight = request.Weight,
        };
    }

    internal static UpdateComponentRequest ToUpdateComponentRequest(
        this UpdateComponentApiRequest request)
    {
        return new UpdateComponentRequest
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            Weight = request.Weight,
        };
    }
}