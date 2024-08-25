using DataContracts;

namespace Contracts.ProjectEntities;

/// <summary>
/// Контрагент
/// </summary>
public class Counteragent : DatabaseEntity
{
    /// <summary>
    /// Название организации-контрагента
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Контактное лицо организации-контрагента
    /// </summary>
    public string Contact { get; set; }

    /// <summary>
    /// Телефон контактного лица организации-контрагента
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// ИНН организации-контрагента
    /// </summary>
    public uint? INN { get; set; }

    /// <summary>
    /// ОГРН организации-контрагента
    /// </summary>
    public uint? OGRN { get; set; }

    /// <summary>
    /// Банковский счет организации-контрагента
    /// </summary>
    public ulong? AccountNumber { get; set; }

    /// <summary>
    /// БИК счета организации-контрагента
    /// </summary>
    public uint? BIK { get; set; }
}