using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProductComponentService : IProductComponentService
{
    private readonly IRepository<ProductComponent> _productComponentRepository;
    private readonly ILogger<ProductComponentService> _logger;

    public ProductComponentService(
        IRepository<ProductComponent> productComponentRepository,
        ILogger<ProductComponentService> logger)
    {
        _productComponentRepository = productComponentRepository;
        _logger = logger;
    }

   public async Task<ProductComponent> CreateProductComponentAsync(
       CreateProductComponentRequest request, 
       CancellationToken cancellationToken)
    {
        var createdProductComponent = new ProductComponent
        {
            Product = request.Product,
            Name = request.Name,
            Quantity = request.Quantity,
            Weight = request.Weight
        };
        
        _logger.LogInformation("Компонент изделия успешно добавлен: {@ProductComponent}", 
            createdProductComponent);
        return createdProductComponent;
    }
    
    public async Task<ProductComponent?> UpdateProductComponentAsync(
        UpdateProductComponentRequest request,
        CancellationToken cancellationToken)
    {
        var productComponent = await _productComponentRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (productComponent == null) return null;

        productComponent.Id = request.Id;
        productComponent.Name = request.Name;
        productComponent.Product = request.Product;
        productComponent.Quantity = request.Quantity;
        productComponent.Weight = request.Weight;

        await _productComponentRepository.UpdateAsync(productComponent, cancellationToken);

        _logger.LogInformation("Компонент изделия успешно обновлен: {@ProductComponent}", productComponent);
        return productComponent;
    }
    
    public async Task<ProductComponent?> GetProductComponentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _productComponentRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
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
}