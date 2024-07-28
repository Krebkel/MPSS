namespace Web.Requests.ProductRequests;

public class UpdateProductComponentApiRequest
{
    public required int ProductId { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}