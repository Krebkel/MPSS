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
        ValidateProjectProduct(projectProduct);
        
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
    public List<ProjectProduct> GetAllProjectProducts(int projectId)
    {
        return _context.ProjectProducts
            .Where(es => es.ProjectId == projectId)
            .ToList();
    }
    
    /// <inheritdoc />
    public void UpdateProjectProduct(ProjectProduct projectProduct)
    {
        ValidateProjectProduct(projectProduct);
        
        var existingProjectProduct = _context.ProjectProducts.Find(projectProduct.Id);
    
        if (existingProjectProduct != null)
        {
            existingProjectProduct.ProjectId = projectProduct.ProjectId;
            existingProjectProduct.ProductId = projectProduct.ProductId;
            existingProjectProduct.Quantity = projectProduct.Quantity;
            existingProjectProduct.Markup = projectProduct.Markup;

            _context.SaveChanges();
        }
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
    
    private void ValidateProjectProduct(ProjectProduct projectProduct)
    {
        if (projectProduct.ProjectId <= 0)
        {
            throw new ArgumentException("Ошибка в выборе проекта.");
        }

        if (projectProduct.ProductId <= 0)
        {
            throw new ArgumentException("Ошибка в выборе изделия.");
        }

        if (projectProduct.Quantity <= 0)
        {
            throw new ArgumentException("Некорректное количество изделий.");
        }

        if (projectProduct.Markup < 0)
        {
            throw new ArgumentException("Некорректная наценка.");
        }
    }
}