namespace Web.Requests.ProductRequests;

public class CreateProjectProductApiRequest
{
    public required int Project { get; set; }
    public required int Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}