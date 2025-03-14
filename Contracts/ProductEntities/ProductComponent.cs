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
    /// Компонент производимой работы
    /// </summary>
    public Component Component { get; set; }
    
    /// <summary>
    /// Количество единиц компонентов работы
    /// </summary>
    public double Quantity { get; set; }
}