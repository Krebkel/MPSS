using Contracts.EmployeeEntities;

namespace Employees.Services;

/// <summary>
/// Сервис для управления сменами сотрудников
/// </summary>
public interface IEmployeeShiftService
{
    /// <summary>
    /// Добавление смены сотрудника в базу данных
    /// </summary>
    /// <param name="employeeShift">Смена сотрудника</param>
    /// <returns>ID созданной смены</returns>
    int CreateEmployeeShift(EmployeeShift employeeShift);

    /// <summary>
    /// Получение данных смены сотрудника из базы данных
    /// </summary>
    /// <param name="employeeShiftId">ID смены сотрудника</param>
    /// <returns>Смена</returns>
    EmployeeShift GetEmployeeShift(int employeeShiftId);

    /// <summary>
    /// Получение данных всех смен сотрудника из базы данных
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <returns>Список смен</returns>
    List<EmployeeShift> GetAllEmployeeShifts(int employeeId);

    /// <summary>
    /// Обновление данных смены сотрудника в базе данных
    /// </summary>
    /// <param name="employeeShift">Смена сотрудника</param>
    void UpdateEmployeeShift(EmployeeShift employeeShift);

    /// <summary>
    /// Удаление смены сотрудника из базы данных
    /// </summary>
    /// <param name="employeeShiftId">ID смены сотрудника</param>
    void DeleteEmployeeShift(int employeeShiftId);

    /// <summary>
    /// Расчет времени смены в часах по времени прибытия на объект и отъезда с него
    /// </summary>
    /// <param name="arrival">Время прибытия</param>
    /// <param name="departure">Время отъезда</param>
    /// <returns>Общее время смены в часах</returns>
    float CalculateTotalTime(DateTimeOffset? arrival, DateTimeOffset? departure);

    /// <summary>
    /// Расчет зарплаты за проект сотрудника
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <param name="projectId">ID проекта</param>
    /// <returns></returns>
    double CalculateTotalWage(int employeeId, int projectId);
    
    /// <summary>
    /// Расчет зарплаты сотрудника
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <returns></returns>
    double CalculateTotalWageForDoneProjects(int employeeId);
}