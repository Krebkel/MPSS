using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Производимый вид работ
/// </summary>
public class Product : DatabaseEntity
{
    /// <summary>
    /// Наименование работы
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Стоимость единицы работы в рублях
    /// </summary>
    public double Cost { get; set; }
    
    /// <summary>
    /// Вид работы
    /// </summary>
    public ProductType Type { get; set; }
}