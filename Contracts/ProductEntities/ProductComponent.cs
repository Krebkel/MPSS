using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Детали производимого продукта
/// </summary>
public class ProductComponent : DatabaseEntity
{
    /// <summary>
    /// ID производимого продукта
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Наименование производимого продукта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество деталей для производства продукта в штуках
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Вес одной детали для производства продукта
    /// </summary>
    public double? Weight { get; set; }
}