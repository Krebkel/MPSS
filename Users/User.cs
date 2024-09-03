using Contracts.EmployeeEntities;
using DataContracts;

namespace Users;

/// <summary>
/// Пользователь сервиса
/// </summary>
public class User : DatabaseEntity
{
    /// <summary>
    /// Физ-лицо
    /// </summary>
    public required Employee Employee { get; set; }

    /// <summary>
    /// Пароль TODO на хеш
    /// </summary>
    public required string Password { get; set; }
}