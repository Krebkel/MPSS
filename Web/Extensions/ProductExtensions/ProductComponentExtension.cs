using Contracts.ProductEntities;
using Web.Requests.ProductRequests;
using Web.Responses.ProductResponses;

namespace Web.Extensions.ProductExtensions;

public static class ProductComponentExtension
{
    public static ProductComponent ToProductComponent(this CreateProductComponentApiRequest apiRequest)
    {
        return new ProductComponent
        {
            ProductId = apiRequest.ProductId,
            Name = apiRequest.Name,
            Quantity = apiRequest.Quantity,
            Weight = apiRequest.Weight
        };
    }

    public static ProductComponent ToProductComponent(this UpdateProductComponentApiRequest apiRequest, int id)
    {
        return new ProductComponent
        {
            Id = id,
            ProductId = apiRequest.ProductId,
            Name = apiRequest.Name,
            Quantity = apiRequest.Quantity,
            Weight = apiRequest.Weight
        };
    }

    public static ApiProductComponent ToApiProductComponent(this ProductComponent productComponent)
    {
        return new ApiProductComponent
        {
            Id = productComponent.Id,
            ProductId = productComponent.ProductId,
            Name = productComponent.Name,
            Quantity = productComponent.Quantity,
            Weight = productComponent.Weight
        };
    }
}