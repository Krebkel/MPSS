using Contracts.ProductEntities;
using Web.Requests;
using Web.Requests.ProductRequests;
using Web.Responses;
using Web.Responses.ProductResponses;

namespace Web.Extensions.ProductExtensions;

public static class ProductExtension
{
    public static Product ToProduct(this CreateProductApiRequest apiRequest)
    {
        return new Product
        {
            Name = apiRequest.Name,
            Cost = apiRequest.Cost
        };
    }

    public static Product ToProduct(this UpdateProductApiRequest apiRequest, int id)
    {
        return new Product
        {
            Id = id,
            Name = apiRequest.Name,
            Cost = apiRequest.Cost
        };
    }

    public static ApiProduct ToApiProduct(this Product product)
    {
        return new ApiProduct
        {
            Id = product.Id,
            Name = product.Name,
            Cost = product.Cost
        };
    }
}