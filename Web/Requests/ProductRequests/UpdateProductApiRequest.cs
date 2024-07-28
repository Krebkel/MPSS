namespace Web.Requests.ProductRequests;

public class UpdateProductApiRequest
{
    public required string Name { get; set; }
    public required double Cost { get; set; }
}