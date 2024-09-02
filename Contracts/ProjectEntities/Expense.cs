using Contracts.EmployeeEntities;
using DataContracts;

namespace Contracts.ProjectEntities;

/// <summary>
/// Дополнительные расходы на закрытие проекта
/// </summary>
public class Expense : DatabaseEntity
{
    /// <summary>
    /// ID проекта
    /// </summary>
    public Project Project { get; set; }

    /// <summary>
    /// Назначение расхода
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество единиц
    /// </summary>
    public double? Amount { get; set; }

    /// <summary>
    /// Описание расхода
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Тип расхода
    /// </summary>
    public ExpenseType Type { get; set; }
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Employee? Employee { get; set; }
    
    /// <summary>
    /// Оплачено ли заказчиком
    /// </summary>
    public bool IsPaidByCompany { get; set; }
}