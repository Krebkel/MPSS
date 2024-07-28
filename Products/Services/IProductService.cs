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
    /// Обновление данных изделия в базе данных
    /// </summary>
    /// <param name="product">Изделие</param>
    void UpdateProduct(Product product);

    /// <summary>
    /// Обновление данных изделия в базе данных
    /// </summary>
    /// <param name="productId">ID изделия</param>
    public void DeleteProduct(int productId);

    /// <summary>
    /// Получение данных изделия из базы данных
    /// </summary>
    /// <param name="productId">ID изделия</param>
    public Product GetProduct(int productId);

}