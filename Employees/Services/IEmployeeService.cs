using Contracts.EmployeeEntities;

namespace Employees.Services;

/// <summary>
/// Интерфейс сервиса для работы с сотрудниками.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Добавление сотрудника в базу данных
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    /// <returns>ID созданного сотрудника</returns>
    int CreateEmployee(Employee employee);

    /// <summary>
    /// Обновление данных сотрудника в базе данных
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    void UpdateEmployee(Employee employee);

    /// <summary>
    /// Назначение смены сотруднику
    /// </summary>
    /// <param name="employeeShift">Смена сотрудника</param>
    /// <returns>ID созданной смены</returns>
    int AssignShift(EmployeeShift employeeShift);

    /// <summary>
    /// Расчет зарплаты сотрудника за проект
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <param name="projectId">ID проекта</param>
    /// <returns>Сумма заработной платы</returns>
    double CalculateWage(int employeeId, int projectId);

    /// <summary>
    /// Расчет зарплаты за смену для сотрудника
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <param name="employeeShiftId">ID смены</param>
    /// <returns>Сумма заработной платы за смену</returns>
    double CalculateShiftWage(int employeeId, int employeeShiftId);
}