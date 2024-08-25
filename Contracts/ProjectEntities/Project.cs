using Contracts.EmployeeEntities;
using DataContracts;

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
    /// ID организации-заказчика
    /// </summary>
    public Counteragent? Counteragent { get; set; }

    /// <summary>
    /// Ответственный сотрудник
    /// </summary>
    public Employee ResponsibleEmployee { get; set; }
    
    /// <summary>
    /// Процент, который получает руководитель от прибыли
    /// </summary>
    public float ManagerShare { get; set; }

    /// <summary>
    /// Статус проекта
    /// </summary>
    public ProjectStatus ProjectStatus { get; set; }
}