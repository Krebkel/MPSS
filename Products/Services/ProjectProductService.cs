using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using Contracts.ProjectEntities;
using Data;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProjectProductService : IProjectProductService
{
    private readonly IRepository<ProjectProduct> _projectProductRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<ProjectProductService> _logger;
    private readonly IValidator<Project> _projectValidator;
    private readonly IValidator<Product> _productValidator;
    private readonly IValidator<ProjectProduct> _projectProductValidator;

    public ProjectProductService(
        IRepository<ProjectProduct> projectProductRepository,
        IRepository<Project> projectRepository,
        IRepository<Product> productRepository,
        ILogger<ProjectProductService> logger,
        IValidator<Project> projectValidator,
        IValidator<Product> productValidator,
        IValidator<ProjectProduct> projectProductValidator)
    {
        _projectProductRepository = projectProductRepository;
        _projectRepository = projectRepository;
        _productRepository = productRepository;
        _logger = logger;
        _projectValidator = projectValidator;
        _productValidator = productValidator;
        _projectProductValidator = projectProductValidator;
    }

    public async Task<ProjectProduct> CreateProjectProductAsync(CreateProjectProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.Project,
            _projectRepository, 
            "Проект", 
            cancellationToken);
        
        var product = await _productValidator.ValidateAndGetEntityAsync(
            request.Product,
            _productRepository, 
            "Изделие",
            cancellationToken);

        var createdProjectProduct = new ProjectProduct
        {
            Project = project,
            Product = product,
            Quantity = request.Quantity,
            Markup = request.Markup
        };

        await _projectProductRepository.AddAsync(createdProjectProduct, cancellationToken);
        _logger.LogInformation("Изделие на проекте успешно добавлено: {@ProjectProduct}", createdProjectProduct);

        return createdProjectProduct;
    }
    
    public async Task<ProjectProduct> UpdateProjectProductAsync(
        UpdateProjectProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var projectProduct = await _projectProductValidator.ValidateAndGetEntityAsync(request.Id,
            _projectProductRepository, "Проектное мзделме", cancellationToken);
        
        var project = await _projectValidator.ValidateAndGetEntityAsync(request.Project,
            _projectRepository, "Проект", cancellationToken);
        
        var product = await _productValidator.ValidateAndGetEntityAsync(request.Product,
            _productRepository, "Изделие", cancellationToken);
        

        projectProduct.Project = project;
        projectProduct.Product = product;
        projectProduct.Quantity = request.Quantity;
        projectProduct.Markup = request.Markup;

        await _projectProductRepository.UpdateAsync(projectProduct, cancellationToken);
        _logger.LogInformation("Изделие на проекте успешно обновлено: {@ProjectProduct}", projectProduct);

        return projectProduct;
    }
    
    public async Task<object?> GetProjectProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _projectProductRepository
            .GetAll()
            .Select(pp => new
            {
                Id = pp.Id,
                Project = pp.Project.Id,
                Product = pp.Product.Id,
                Quantity = pp.Quantity,
                Markup = pp.Markup
            })
            .FirstOrDefaultAsync(pp => pp.Id == id, cancellationToken);
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
    
    public async Task<IEnumerable<object>> GetProjectProductsByProjectIdAsync(
        int projectId, CancellationToken cancellationToken)
    {
        var projectExists = await _projectRepository.GetAll()
            .AnyAsync(p => p.Id == projectId, cancellationToken);

        if (!projectExists)
        {
            _logger.LogWarning("Проект с ID {ProjectId} не найден", projectId);
            throw new KeyNotFoundException($"Проект с ID {projectId} не найден");
        }

        return await _projectProductRepository
            .GetAll()
            .Where(pp => pp.Project.Id == projectId)
            .Select(pp => new
            {
                Id = pp.Id,
                Project = pp.Project.Id,
                Product = pp.Product.Id,
                Quantity = pp.Quantity,
                Markup = pp.Markup
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<object>> GetRecentProjectProductsByProductIdAsync(
        int productId,
        int limit,
        CancellationToken cancellationToken)
    {
        try
        {
            var productExists = await _productRepository.GetAll()
                .AnyAsync(p => p.Id == productId, cancellationToken);

            if (!productExists)
            {
                _logger.LogWarning("Изделие с ID {ProductId} не найдено", productId);
                throw new KeyNotFoundException($"Изделие с ID {productId} не найдено");
            }
            
            return await _projectProductRepository
                .GetAll()
                .Where(pp => pp.Product.Id == productId)
                .OrderByDescending(pp => pp.Id)
                .Take(limit)
                .Select(pp => new
                {
                    Id = pp.Id,
                    Project = pp.Project.Id,
                    Product = pp.Product.Id,
                    Quantity = pp.Quantity,
                    Markup = pp.Markup
                })
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении последних ProjectProducts для Product с ID {ProductId}",
                productId);
            throw;
        }
    }
}