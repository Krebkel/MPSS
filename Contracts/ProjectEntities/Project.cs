namespace Contracts.ProjectEntities;

/// <summary>
/// Проект
/// </summary>
public class Project : DatabaseEntity
{
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Адрес объекта
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Дата дедлайна сдачи объекта
    /// </summary>
    public DateTimeOffset DeadlineDate { get; set; }

    /// <summary>
    /// Дата начала работ на объекта
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Дата приостановки работы на объекте
    /// </summary>
    public DateTimeOffset? DateSuspended { get; set; }

    /// <summary>
    /// ID организации-заказчика
    /// </summary>
    public int? CounteragentId { get; set; }

    /// <summary>
    /// Суммарная стоимость проекта
    /// </summary>
    public double TotalCost { get; set; }

    /// <summary>
    /// Ответственный сотрудник
    /// </summary>
    public int ResponsibleEmployeeId { get; set; }

    /// <summary>
    /// Проект в работе
    /// </summary>
    public bool IsActive { get; set; }
}