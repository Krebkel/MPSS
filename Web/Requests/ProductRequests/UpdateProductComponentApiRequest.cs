namespace Web.Requests.ProductRequests;

public class UpdateProductComponentApiRequest
{
    public required int Id { get; set; }
    public required int Product { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}