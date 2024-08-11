using Contracts.ProductEntities;

namespace Products.Services;

/// <summary>
/// Интерфейс сервиса для работы с изделиями.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Добавление изделия в базу данных
    /// </summary>
    /// <param name="product">Изделие</param>
    /// <returns>ID созданного изделия</returns>
    int CreateProduct(Product product);
    
    /// <summary>
    /// Получение данных изделия из базы данных
    /// </summary>
    /// <param name="productId">ID изделия</param>
    Product GetProduct(int productId);

    /// <summary>
    /// Получение данных о всех изделиях из базы данных
    /// </summary>
    List<Product> GetAllProducts();

    /// <summary>
    /// Обновление данных изделия в базе данных
    /// </summary>
    /// <param name="product">Изделие</param>
    void UpdateProduct(Product product);

    /// <summary>
    /// Обновление данных изделия в базе данных
    /// </summary>
    /// <param name="productId">ID изделия</param>
    void DeleteProduct(int productId);
}