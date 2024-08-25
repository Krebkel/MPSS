using Contracts;

namespace Web.Requests.ProductRequests;

public class UpdateProductApiRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Cost { get; set; }
    public required ProductType Type { get; set; }
}