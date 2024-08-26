using System.ComponentModel.DataAnnotations.Schema;

namespace DataContracts;

/// <summary>
/// Базовый класс для сущности
/// </summary>
public abstract class DatabaseEntity
{
    /// <summary>
    /// ID сущности
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}