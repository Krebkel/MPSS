using Contracts.ProductEntities;
using Data;

namespace Products.Services;

public class ProjectProductService : IProjectProductService
{
    private readonly AppDbContext _context;

    public ProjectProductService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateProjectProduct(ProjectProduct projectProduct)
    {
        _context.ProjectProducts.Add(projectProduct);
        _context.SaveChanges();
        return projectProduct.Id;
    }

    /// <inheritdoc />
    public ProjectProduct GetProjectProduct(int projectProductId)
    {
        return _context.ProjectProducts.Find(projectProductId);
    }
    
    /// <inheritdoc />
    public void UpdateProjectProduct(ProjectProduct projectProduct)
    {
        _context.ProjectProducts.Update(projectProduct);
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public void DeleteProjectProduct(int projectProductId)
    {
        var projectProduct = _context.ProjectProducts.Find(projectProductId);
        if (projectProduct != null)
        {
            _context.ProjectProducts.Remove(projectProduct);
            _context.SaveChanges();
        }
    }
}