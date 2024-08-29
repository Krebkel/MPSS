using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using Data;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProductComponentService : IProductComponentService
{
    private readonly IRepository<ProductComponent> _productComponentRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<ProductComponentService> _logger;
    private readonly IValidator<Product> _productValidator;
    private readonly IValidator<ProductComponent> _productComponentValidator;


    public ProductComponentService(
        IRepository<ProductComponent> productComponentRepository,
        IRepository<Product> productRepository,
        ILogger<ProductComponentService> logger,
        IValidator<Product> productValidator,
        IValidator<ProductComponent> productComponentValidator)
    {
        _productComponentRepository = productComponentRepository;
        _productRepository = productRepository;
        _logger = logger;
        _productValidator = productValidator;
        _productComponentValidator = productComponentValidator;
    }

    public async Task<ProductComponent> CreateProductComponentAsync(CreateProductComponentRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var product = await _productValidator.ValidateAndGetEntityAsync(request.Product,
            _productRepository, "Изделие", cancellationToken);

        var createdProductComponent = new ProductComponent
        {
            Product = product,
            Name = request.Name,
            Quantity = request.Quantity,
            Weight = request.Weight
        };

        await _productComponentRepository.AddAsync(createdProductComponent, cancellationToken);
        _logger.LogInformation("Компонент изделия успешно добавлен: {@ProductComponent}", createdProductComponent);

        return createdProductComponent;
    }
    
    public async Task<ProductComponent> UpdateProductComponentAsync(UpdateProductComponentRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var productComponent = await _productComponentValidator.ValidateAndGetEntityAsync(request.Id,
            _productComponentRepository, "Компонент изделия", cancellationToken);
        
        var product = await _productValidator.ValidateAndGetEntityAsync(request.Product,
            _productRepository, "Изделие", cancellationToken);

        productComponent.Name = request.Name;
        productComponent.Product = product;
        productComponent.Quantity = request.Quantity;
        productComponent.Weight = request.Weight;

        await _productComponentRepository.UpdateAsync(productComponent, cancellationToken);
        _logger.LogInformation("Компонент изделия успешно обновлен: {@ProductComponent}", productComponent);

        return productComponent;
    }
    
    public async Task<object?> GetProductComponentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _productComponentRepository
            .GetAll()
            .Select(pc => new
            {
                Id = pc.Id,
                Product = pc.Product.Id,
                Name = pc.Name,
                Quantity = pc.Quantity,
                Weight = pc.Weight
            })
            .FirstOrDefaultAsync(pc => pc.Id == id, cancellationToken);
    }


    public async Task<bool> DeleteProductComponentAsync(int id, CancellationToken cancellationToken)
    {
        var productComponent = await _productComponentRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (productComponent == null) return false;

        await _productComponentRepository.DeleteAsync(productComponent, cancellationToken);
        _logger.LogInformation("Компонент изделия с ID {Id} успешно удален", id);
        return true;
    }
    
    public async Task<IEnumerable<object>> GetProductComponentsByProductIdAsync(
        int productId, CancellationToken cancellationToken)
    {
        var productExists = await _productRepository.GetAll()
            .AnyAsync(p => p.Id == productId, cancellationToken);

        if (!productExists)
        {
            _logger.LogWarning("Изделие с ID {ProductId} не найдено", productId);
            throw new KeyNotFoundException($"Изделие с ID {productId} не найдено");
        }

        return await _productComponentRepository
            .GetAll()
            .Where(pc => pc.Product.Id == productId)
            .Select(pc => new
            {
                Id = pc.Id,
                Product = pc.Product.Id,
                Name = pc.Name,
                Quantity = pc.Quantity,
                Weight = pc.Weight
            })
            .ToListAsync(cancellationToken);
    }

}