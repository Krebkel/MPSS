using Contracts.ProductEntities;

namespace Products.Services;

public interface IProductComponentService
{
    Task<ProductComponent> CreateProductComponentAsync(CreateProductComponentRequest productComponent, CancellationToken cancellationToken);
    
    Task<ProductComponent> UpdateProductComponentAsync(UpdateProductComponentRequest productComponent, CancellationToken cancellationToken);
    
    Task<ProductComponent?> GetProductComponentByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductComponentAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<ProductComponent>> GetProductComponentsByProductIdAsync(int productId, CancellationToken cancellationToken);
}

public class UpdateProductComponentRequest
{
    public required int Id { get; set; }
    public required int Product { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}

public class CreateProductComponentRequest
{
    public required int Product { get; set; }
    public required string Name { get; set; }
    public int? Quantity { get; set; }
    public float? Weight { get; set; }
}