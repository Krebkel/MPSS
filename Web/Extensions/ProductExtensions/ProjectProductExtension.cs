using Contracts.ProductEntities;
using Web.Requests.ProductRequests;
using Web.Responses.ProductResponses;

namespace Web.Extensions.ProductExtensions;

public static class ProjectProductExtension
{
    public static ProjectProduct ToProjectProduct(this CreateProjectProductApiRequest apiRequest)
    {
        return new ProjectProduct
        {
            ProjectId = apiRequest.ProjectId,
            ProductId = apiRequest.ProductId,
            Quantity = apiRequest.Quantity,
            Markup = apiRequest.Markup
        };
    }

    public static ProjectProduct ToProjectProduct(this UpdateProjectProductApiRequest apiRequest, int id)
    {
        return new ProjectProduct
        {
            Id = id,
            ProjectId = apiRequest.ProjectId,
            ProductId = apiRequest.ProductId,
            Quantity = apiRequest.Quantity,
            Markup = apiRequest.Markup
        };
    }

    public static ApiProjectProduct ToApiProjectProduct(this ProjectProduct projectProduct)
    {
        return new ApiProjectProduct
        {
            Id = projectProduct.Id,
            ProjectId = projectProduct.ProjectId,
            ProductId = projectProduct.ProductId,
            Quantity = projectProduct.Quantity,
            Markup = projectProduct.Markup
        };
    }
}