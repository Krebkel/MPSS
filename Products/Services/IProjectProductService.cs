using Contracts.ProductEntities;

namespace Products.Services;

public interface IProjectProductService
{
    Task<ProjectProduct> CreateProjectProductAsync(CreateProjectProductRequest projectProduct,
        CancellationToken cancellationToken);

    Task<ProjectProduct> UpdateProjectProductAsync(UpdateProjectProductRequest projectProduct,
        CancellationToken cancellationToken);
    
    Task<object?> GetProjectProductByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectProductAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<object>> GetProjectProductsByProjectIdAsync(int projectId,
        CancellationToken cancellationToken);
}

public class UpdateProjectProductRequest
{
    public required int Id { get; set; }
    public required int Project { get; set; }
    public required int Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}

public class CreateProjectProductRequest
{
    public required int Project { get; set; }
    public required int Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}