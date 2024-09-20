namespace Web.Requests.ProductRequests;

public class UpdateProductComponentApiRequest
{
    public required int Id { get; set; }
    public required int Product { get; set; }
    public required int Component { get; set; }
    public double Quantity { get; set; }
}