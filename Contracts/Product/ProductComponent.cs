namespace Contracts.Product;

/// <summary>
/// Детали производимого продукта
/// </summary>
public class ProductComponent : DatabaseEntity
{
    /// <summary>
    /// ID производимого продукта
    /// </summary>
    public int ProductId { get; set; }

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
    public float? Weight { get; set; }
}