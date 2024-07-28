namespace Web.Responses.ProductResponses;

public class ApiProjectProduct
{
    public required int Id { get; set; }
    public required int ProjectId { get; set; }
    public required int ProductId { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}