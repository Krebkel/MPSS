namespace Web.Requests.ProductRequests;

public class CreateProductApiRequest
{
    public required string Name { get; set; }
    public required double Cost { get; set; }
}