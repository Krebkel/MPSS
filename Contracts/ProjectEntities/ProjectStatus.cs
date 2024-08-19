namespace Contracts.ProjectEntities;

public enum ProjectStatus
{
    /// <summary>
    /// В работе
    /// </summary>
    Active,
    
    /// <summary>
    /// Приостановлен
    /// </summary>
    Standby,
    
    /// <summary>
    /// Выполнен
    /// </summary>
    Done,
    
    /// <summary>
    /// Рассчитан
    /// </summary>
    Paid
}