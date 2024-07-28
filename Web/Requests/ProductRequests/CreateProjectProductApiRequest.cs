namespace Web.Requests.ProductRequests;

public class CreateProjectProductApiRequest
{
    public required int ProjectId { get; set; }
    public required int ProductId { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}