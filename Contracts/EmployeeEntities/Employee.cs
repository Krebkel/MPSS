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
    public ulong? Passport { get; set; }

    /// <summary>
    /// Дата рождения сотрудника
    /// </summary>
    public DateTimeOffset DateOfBirth { get; set; }

    /// <summary>
    /// ИНН сотрудника
    /// </summary>
    public ulong? INN { get; set; }

    /// <summary>
    /// Банковский счет сотрудника
    /// </summary>
    public ulong? AccountNumber { get; set; }

    /// <summary>
    /// БИК счета сотудника
    /// </summary>
    public ulong? BIK { get; set; }
}