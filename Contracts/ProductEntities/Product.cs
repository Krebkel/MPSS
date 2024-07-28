namespace Contracts.ProductEntities;

/// <summary>
/// Производимый продукт
/// </summary>
public class Product : DatabaseEntity
{
    /// <summary>
    /// Наименование продукта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Стоимость единицы в рублях
    /// </summary>
    public double Cost { get; set; }
}