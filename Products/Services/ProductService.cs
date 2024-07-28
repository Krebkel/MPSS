using Contracts.ProductEntities;
using Data;

namespace Products.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return product.Id;
    }
    
    /// <inheritdoc />
    public Product GetProduct(int productId)
    {
        return _context.Products.Find(productId);
    }

    /// <inheritdoc />
    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public void DeleteProduct(int productId)
    {
        var product = _context.Products.Find(productId);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}