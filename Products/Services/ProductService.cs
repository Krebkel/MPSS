using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IValidator<Product> _productValidator;

    public ProductService(
        IRepository<Product> productRepository,
        ILogger<ProductService> logger,
        IValidator<Product> productValidator)
    {
        _productRepository = productRepository;
        _logger = logger;
        _productValidator = productValidator;
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var createdProduct = new Product
        {
            Name = request.Name,
            Cost = request.Cost,
            Type = request.Type
        };
        
        await _productRepository.AddAsync(createdProduct, cancellationToken);

        _logger.LogInformation("Изделие успешно добавлено: {@Product}", createdProduct);
        return createdProduct;
    }
    
    public async Task<Product> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var product = await _productValidator.ValidateAndGetEntityAsync(
            request.Id,
            _productRepository,
            "Изделие",
            cancellationToken);

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
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAll().ToListAsync(cancellationToken);
    }
}