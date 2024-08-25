namespace DataContracts;

/// <summary>
/// Базовый класс для сущности
/// </summary>
public abstract class DatabaseEntity
{
    /// <summary>
    /// ID сущности
    /// </summary>
    public int Id { get; set; }
}