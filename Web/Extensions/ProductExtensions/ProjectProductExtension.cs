using Products.Services;
using Web.Requests.ProductRequests;

namespace Web.Extensions.ProductExtensions;

public static class ProjectProductExtension
{
    internal static CreateProjectProductRequest ToCreateProjectProductRequest(
        this CreateProjectProductApiRequest request)
    {
        return new CreateProjectProductRequest
        {
            Project = request.Project,
            Product = request.Product,
            Quantity = request.Quantity,
            Markup = request.Markup
        };
    }
    
    internal static UpdateProjectProductRequest ToUpdateProjectProductRequest(
        this UpdateProjectProductApiRequest request)
    {
        return new UpdateProjectProductRequest
        {
            Id = request.Id,
            Project = request.Project,
            Product = request.Product,
            Quantity = request.Quantity,
            Markup = request.Markup
        };
    }
}