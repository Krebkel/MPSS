using Contracts.ProductEntities;
using Contracts.ProjectEntities;

namespace Products.Services;

public interface IProjectProductService
{
    Task<ProjectProduct> CreateProjectProductAsync(CreateProjectProductRequest projectProduct, CancellationToken cancellationToken);
    
    Task<ProjectProduct> UpdateProjectProductAsync(UpdateProjectProductRequest projectProduct, CancellationToken cancellationToken);
    
    Task<ProjectProduct?> GetProjectProductByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectProductAsync(int id, CancellationToken cancellationToken);
}

public class UpdateProjectProductRequest
{
    public required int Id { get; set; }
    public required Project Project { get; set; }
    public required Product Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}

public class CreateProjectProductRequest
{
    public required Project Project { get; set; }
    public required Product Product { get; set; }
    public required int Quantity { get; set; }
    public required double Markup { get; set; }
}