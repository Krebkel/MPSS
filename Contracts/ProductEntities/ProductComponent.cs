using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Компоненты производимой работы
/// </summary>
public class ProductComponent : DatabaseEntity
{
    /// <summary>
    /// ID производимой работы
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Наименование производимой работы
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество компонентов для выполнения работы
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Вес одного компонента для выполнения работы
    /// </summary>
    public double? Weight { get; set; }
}