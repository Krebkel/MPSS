using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Производимое изделие
/// </summary>
public class Product : DatabaseEntity
{
    /// <summary>
    /// Наименование изделия
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Стоимость единицы в рублях
    /// </summary>
    public double Cost { get; set; }
    
    /// <summary>
    /// Вид изделия
    /// </summary>
    public ProductType Type { get; set; }
}