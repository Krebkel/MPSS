using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IRepository<Product> productRepository,
        ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var createdProduct = new Product
        {
            Name = request.Name,
            Cost = request.Cost,
            Type = request.Type
        };
        
        _logger.LogInformation("Изделие успешно добавлено: {@Product}", createdProduct);
        return createdProduct;
    }
    
    public async Task<Product?> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (product == null) return null;

        product.Name = request.Name;
        product.Cost = request.Cost;
        product.Type = request.Type;

        await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation("Изделин успешно обновлено: {@Product}", product);
        return product;
    }
    
    public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _productRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (product == null) return false;

        await _productRepository.DeleteAsync(product, cancellationToken);
        _logger.LogInformation("Изделие с ID {Id} успешно удалено", id);
        return true;
    }
}