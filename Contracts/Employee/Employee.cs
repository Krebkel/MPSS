namespace Contracts.Employee;

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
    public uint? Passport { get; set; }

    /// <summary>
    /// Дата рождения сотрудника
    /// </summary>
    public DateTimeOffset DateOfBirth { get; set; }

    /// <summary>
    /// ИНН сотрудника
    /// </summary>
    public uint? INN { get; set; }

    /// <summary>
    /// Банковский счет сотрудника
    /// </summary>
    public ulong? AccountNumber { get; set; }

    /// <summary>
    /// БИК счета сотудника
    /// </summary>
    public uint? BIK { get; set; }
}