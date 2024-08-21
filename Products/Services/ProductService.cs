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
        ValidateProduct(product);
        
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
    public List<Product> GetAllProducts()
    {
        return _context.Products.OrderBy(p=>p.Name).ToList();
    }

    /// <inheritdoc />
    public void UpdateProduct(Product product)
    {
        ValidateProduct(product);
        
        var existingProduct = _context.Products.Find(product.Id);
    
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name ?? existingProduct.Name;
            existingProduct.Cost = product.Cost != default ? product.Cost : existingProduct.Cost;

            _context.SaveChanges();
        }
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
    
    private void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Ошибка в названии изделия");
        }

        if (product.Cost <= 0)
        {
            throw new ArgumentException("Некорректная стоимость изделия.");
        }
    }

}