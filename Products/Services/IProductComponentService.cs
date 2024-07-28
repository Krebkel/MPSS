using Contracts.ProductEntities;

namespace Products.Services;

public interface IProductComponentService
{
    /// <summary>
    /// Добавление компонента изделия в базу данных
    /// </summary>
    /// <param name="productComponent">Компонент изделия</param>
    /// <returns>ID созданного компонента изделия</returns>
    int CreateProductComponent(ProductComponent productComponent);
    
    /// <summary>
    /// Обновление компонента изделия в базе данных
    /// </summary>
    /// <param name="productComponent">Компонент изделия</param>
    void UpdateProductComponent(ProductComponent productComponent);

    /// <summary>
    /// Удаление компонента изделия из базы данных
    /// </summary>
    /// <param name="productComponentId">ID компонента изделия</param>
    void DeleteProductComponent(int productComponentId);

    /// <summary>
    /// Получение компонента изделия из базы данных
    /// </summary>
    /// <param name="productComponentId">ID компонента изделия</param>
    ProductComponent GetProductComponent(int productComponentId);

    /// <summary>
    /// Расчет веса компонентов изделия по количеству
    /// </summary>
    /// <param name="productId">ID изделия</param>
    /// <returns>Вес в килограммах</returns>
    double CalculateTotalWeight(int productId);
}