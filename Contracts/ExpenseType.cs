namespace Contracts;

/// <summary>
/// Тип расхода
/// </summary>
public enum ExpenseType
{
    /// <summary>
    /// Расходы на дорогу
    /// </summary>
    Travel,

    /// <summary>
    /// Расходные материалы
    /// </summary>
    Wares,
    
    /// <summary>
    /// Расходы сотрудников
    /// </summary>
    Personal,

    /// <summary>
    /// Прочие расходы
    /// </summary>
    Other
}