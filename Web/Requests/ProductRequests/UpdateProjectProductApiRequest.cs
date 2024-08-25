using Contracts.ProductEntities;
using Contracts.ProjectEntities;

namespace Web.Requests.ProductRequests;

public class UpdateProjectProductApiRequest
{
    public required int Id { get; set; }
    public required Project Project { get; set; }
    public required Product Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}