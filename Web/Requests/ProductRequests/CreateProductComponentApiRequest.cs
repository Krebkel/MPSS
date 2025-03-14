namespace Web.Requests.ProductRequests;

public class CreateProductComponentApiRequest
{
    public required int Product { get; set; }
    public required int Component { get; set; }
    public double Quantity { get; set; }
}