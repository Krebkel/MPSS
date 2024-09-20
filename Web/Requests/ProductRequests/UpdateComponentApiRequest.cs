namespace Web.Requests.ProductRequests;

public class UpdateComponentApiRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public double? Price { get; set; }
    public double? Weight { get; set; }
}