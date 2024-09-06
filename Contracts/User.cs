using Contracts.EmployeeEntities;
using DataContracts;

namespace Contracts;

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
    
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public required UserRole Role { get; set; }
}