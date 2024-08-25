using DataContracts;

namespace Contracts.ProjectEntities;

/// <summary>
/// Приостановка выполнения проекта
/// </summary>
public class ProjectSuspension : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// Дата приостановки
    /// </summary>
    public DateTimeOffset DateSuspended { get; set; }
}