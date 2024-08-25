using Contracts.ProductEntities;

namespace Web.Requests.ProductRequests;

public class CreateProductComponentApiRequest
{
    public required Product Product { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}