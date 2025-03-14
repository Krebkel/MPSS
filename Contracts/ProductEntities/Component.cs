using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Компонент работы
/// </summary>
public class Component : DatabaseEntity
{
    /// <summary>
    /// Наименование компонента работы
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Стоимость единицы компонента для выполнения работы
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Вес одного компонента для выполнения работы
    /// </summary>
    public double? Weight { get; set; }
}