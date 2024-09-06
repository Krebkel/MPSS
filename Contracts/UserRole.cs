namespace Contracts;

public enum UserRole
{
    /// <summary>
    /// Администратор базы данных
    /// </summary>
    Service,
    
    /// <summary>
    /// Администратор
    /// </summary>
    Administrator,
    
    /// <summary>
    /// Доверенный
    /// </summary>
    Trusted,
    
    /// <summary>
    /// Обычный
    /// </summary>
    Regular
}