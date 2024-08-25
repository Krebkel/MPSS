using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProjectProductService : IProjectProductService
{
    private readonly IRepository<ProjectProduct> _projectProductRepository;
    private readonly ILogger<ProjectProductService> _logger;

    public ProjectProductService(
        IRepository<ProjectProduct> projectProductRepository,
        ILogger<ProjectProductService> logger)
    {
        _projectProductRepository = projectProductRepository;
        _logger = logger;
    }

    public async Task<ProjectProduct> CreateProjectProductAsync(CreateProjectProductRequest request, CancellationToken cancellationToken)
    {
        var createdProjectProduct = new ProjectProduct
        {
            Project = request.Project,
            Product = request.Product,
            Quantity = request.Quantity,
            Markup = request.Markup
        };
        
        _logger.LogInformation("Изделие успешно добавлено на проект: {@ProjectProduct}", createdProjectProduct);
        return createdProjectProduct;
    }
    
    public async Task<ProjectProduct?> UpdateProjectProductAsync(UpdateProjectProductRequest request, CancellationToken cancellationToken)
    {
        var projectProduct = await _projectProductRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (projectProduct == null) return null;

        projectProduct.Id = request.Id;
        projectProduct.Project = request.Project;
        projectProduct.Product = request.Product;
        projectProduct.Quantity = request.Quantity;
        projectProduct.Markup = request.Markup;

        await _projectProductRepository.UpdateAsync(projectProduct, cancellationToken);

        _logger.LogInformation("Изделие на проекте успешно обновлено: {@ProjectProduct}", projectProduct);
        return projectProduct;
    }
    
    public async Task<ProjectProduct?> GetProjectProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectProductRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteProjectProductAsync(int id, CancellationToken cancellationToken)
    {
        var projectProduct = await _projectProductRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (projectProduct == null) return false;

        await _projectProductRepository.DeleteAsync(projectProduct, cancellationToken);
        _logger.LogInformation("Изделие на проекте с ID {Id} успешно удален", id);
        return true;
    }
}