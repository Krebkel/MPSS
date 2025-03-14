namespace Web.Requests.ProductRequests;

public class CreateComponentApiRequest
{
    public required string Name { get; set; }
    public double? Price { get; set; }
    public double? Weight { get; set; }
}