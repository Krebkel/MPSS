using Contracts.ProductEntities;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Tests.Products;

[TestFixture]
public class ProductTests
{
    private AppDbContext _context;
    private ProductRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductTestDb")
            .Options;

        var dataOptions = Options.Create(new DataOptions { ConnectionString = "InMemoryDbConnectionString", ServiceSchema = "test_schema" });
        
        _context = new AppDbContext(options, dataOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.Products.AddRange(new List<Product>
        {
            new Product
            {
                Id = 1, 
                Name = "Shelf", 
                Cost = 100
            },
            new Product
            {
                Id = 2, 
                Name = "Cabinet", 
                Cost = 200
            }
        });
        _context.SaveChanges();

        _repository = new ProductRepository(_context);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void CreateProduct_ShouldAddProductToDatabase()
    {
        var product = new Product { Name = "Table", Cost = 150 };
            
        var productId = _repository.CreateProduct(product);
        var createdProduct = _context.Products.Find(productId);

        Assert.IsNotNull(createdProduct);
        Assert.AreEqual("Table", createdProduct.Name);
    }

    [Test]
    public void GetProduct_ShouldReturnCorrectProduct()
    {
        var product = _repository.GetProduct(1);
        Assert.IsNotNull(product);
        Assert.AreEqual(1, product.Id);
    }

    [Test]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        var products = _repository.GetAllProducts();
        Assert.AreEqual(2, products.Count);
    }

    [Test]
    public void UpdateProduct_ShouldUpdateProductInDatabase()
    {
        var product = _context.Products.Find(1);
        if (product != null)
        {
            product.Name = "Updated Shelf";
            _repository.UpdateProduct(product);
        }

        var updatedProduct = _context.Products.Find(1);
        Assert.IsNotNull(updatedProduct);
        Assert.AreEqual("Updated Shelf", updatedProduct.Name);
    }

    [Test]
    public void DeleteProduct_ShouldRemoveProductFromDatabase()
    {
        _repository.DeleteProduct(1);
        var deletedProduct = _context.Products.Find(1);
        Assert.IsNull(deletedProduct);
    }
}
