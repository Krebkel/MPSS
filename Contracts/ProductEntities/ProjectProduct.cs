using Contracts.ProjectEntities;
using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Производимая единица работы (Секция стеллажа, ящик, разгрузка etc.) на проекте
/// </summary>
public class ProjectProduct : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// Производимая работа
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Количество единиц работы
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Наценка на одну единицу работы в рублях
    /// </summary>
    public double Markup { get; set; }
}