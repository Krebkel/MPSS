using DataContracts;

namespace Contracts.EmployeeEntities;

/// <summary>
/// Сотрудник
/// </summary>
public class Employee : DatabaseEntity
{
    /// <summary>
    /// ФИО сотудника
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Телефон сотрудника
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Является ли сотрудник водителем
    /// </summary>
    public bool IsDriver { get; set; }

    /// <summary>
    /// Серия и номер паспорта сотрудника
    /// </summary>
    public string? Passport { get; set; }

    /// <summary>
    /// Дата рождения сотрудника
    /// </summary>
    public DateTimeOffset DateOfBirth { get; set; }

    /// <summary>
    /// ИНН сотрудника
    /// </summary>
    public string? INN { get; set; }

    /// <summary>
    /// Банковский счет сотрудника
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// БИК счета сотудника
    /// </summary>
    public string? BIK { get; set; }
}