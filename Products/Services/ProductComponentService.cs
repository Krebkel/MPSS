using Contracts.ProductEntities;
using Data;

namespace Products.Services;

public class ProductComponentService : IProductComponentService
{
    private readonly AppDbContext _context;

    public ProductComponentService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateProductComponent(ProductComponent productComponent)
    {
        _context.ProductComponents.Add(productComponent);
        _context.SaveChanges();
        return productComponent.Id;
    }
    
    /// <inheritdoc />
    public ProductComponent GetProductComponent(int productComponentId)
    {
        return _context.ProductComponents.Find(productComponentId);
    }

    /// <inheritdoc />
    public List<ProductComponent> GetAllProductComponents()
    {
        return _context.ProductComponents.ToList();
    }

    /// <inheritdoc />
    public void UpdateProductComponent(ProductComponent productComponent)
    {
        ValidateProductComponent(productComponent);
            
        var existingProductComponent = _context.ProductComponents.Find(productComponent.Id);

        if (existingProductComponent != null)
        {
            existingProductComponent.ProductId = productComponent.ProductId;
            existingProductComponent.Name = productComponent.Name ?? existingProductComponent.Name;
            existingProductComponent.Quantity = productComponent.Quantity ?? existingProductComponent.Quantity;
            existingProductComponent.Weight = productComponent.Weight ?? existingProductComponent.Weight;

            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public void DeleteProductComponent(int productComponentId)
    {
        var productComponent = _context.ProductComponents.Find(productComponentId);
        if (productComponent != null)
        {
            _context.ProductComponents.Remove(productComponent);
            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public double CalculateTotalWeight(int productId)
    {
        return _context.ProductComponents
            .Where(pc => pc.ProductId == productId)
            .Sum(pc => (pc.Weight ?? 0) * (pc.Quantity ?? 1));
    }
    
    private void ValidateProductComponent(ProductComponent component)
    {
        if (component.ProductId <= 0)
        {
            throw new ArgumentException("Ошибка выбора изделия.");
        }

        if (string.IsNullOrWhiteSpace(component.Name))
        {
            throw new ArgumentException("Ошибка в названии компонента изделия.");
        }

        if (component.Quantity.HasValue && component.Quantity < 0)
        {
            throw new ArgumentException("Некорректное количество компонентов изделия.");
        }
    }
}