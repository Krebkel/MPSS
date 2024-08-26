using Contracts;
using Contracts.ProductEntities;

namespace Products.Services;

/// <summary>
/// Интерфейс сервиса для работы с изделиями.
/// </summary>
public interface IProductService
{
    Task<Product> CreateProductAsync(CreateProductRequest product, CancellationToken cancellationToken);
    
    Task<Product> UpdateProductAsync(UpdateProductRequest product, CancellationToken cancellationToken);
    
    Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
}

public class UpdateProductRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Cost { get; set; }
    public required ProductType Type { get; set; }
}

public class CreateProductRequest
{
    public required string Name { get; set; }
    public required double Cost { get; set; }
    public required ProductType Type { get; set; }
}