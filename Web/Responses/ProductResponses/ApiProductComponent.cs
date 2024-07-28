namespace Web.Responses.ProductResponses;

public class ApiProductComponent
{
    public required int Id { get; set; }
    public required int ProductId { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}