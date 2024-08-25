using Contracts.ProjectEntities;
using DataContracts;

namespace Contracts.EmployeeEntities;

/// <summary>
/// Смена на человека
/// </summary>
public class EmployeeShift : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// ID сотрудника
    /// </summary>
    public Employee Employee { get; set; }

    /// <summary>
    /// Дата смены
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Время появления на объекте
    /// </summary>
    public DateTimeOffset? Arrival { get; set; }

    /// <summary>
    /// Время отбытия с объекта 
    /// </summary>
    public DateTimeOffset? Departure { get; set; }

    /// <summary>
    /// Время проведенное на объекте
    /// </summary>
    public float? HoursWorked { get; set; }

    /// <summary>
    /// Время в дороге
    /// </summary>
    public float? TravelTime { get; set; }

    /// <summary>
    /// Считать ли время в дороге для расчета заработной платы
    /// </summary>
    public bool ConsiderTravel { get; set; }

    /// <summary>
    /// Индивидуальная стимулирующая надбавка
    /// </summary>
    public int? ISN { get; set; }
}