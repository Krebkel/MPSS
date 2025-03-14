using Contracts.ProductEntities;

namespace Products.Services;

public interface IComponentService
{
    Task<Component> CreateComponentAsync(CreateComponentRequest request, CancellationToken cancellationToken);
    Task<Component> UpdateComponentAsync(UpdateComponentRequest request, CancellationToken cancellationToken);
    Task<Component?> GetComponentByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> DeleteComponentAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Component>> GetAllComponentsAsync(CancellationToken cancellationToken);
}

public class CreateComponentRequest
{
    public required string Name { get; set; }
    public double? Price { get; set; }
    public double? Weight { get; set; }
}

public class UpdateComponentRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public double? Price { get; set; }
    public double? Weight { get; set; }
}