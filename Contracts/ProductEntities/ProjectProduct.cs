using Contracts.ProjectEntities;
using DataContracts;

namespace Contracts.ProductEntities;

/// <summary>
/// Производимая единица (Секция стеллажа, ящик, etc.) на проекте
/// </summary>
public class ProjectProduct : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// Производимая единица
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Количество единиц в штуках
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Наценка на одну единицу в рублях
    /// </summary>
    public double Markup { get; set; }
}