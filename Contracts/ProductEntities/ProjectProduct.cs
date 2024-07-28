namespace Contracts.ProductEntities;

/// <summary>
/// Производимая единица (Секция стеллажа, ящик, etc.) на проекте
/// </summary>
public class ProjectProduct : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Производимая единица
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Количество единиц в штуках
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Наценка на одну единицу в рублях
    /// </summary>
    public double Markup { get; set; }
}