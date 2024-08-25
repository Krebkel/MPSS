using Contracts.ProductEntities;
using Contracts.ProjectEntities;

namespace Web.Requests.ProductRequests;

public class CreateProjectProductApiRequest
{
    public required Project Project { get; set; }
    public required Product Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}