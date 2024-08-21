using Contracts.EmployeeEntities;

namespace Employees.Services;

/// <summary>
/// Интерфейс сервиса для работы с сотрудниками.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Создание нового сотрудника в базе данных
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    /// <returns>ID созданного сотрудника</returns>
    int CreateEmployee(Employee employee);

    /// <summary>
    /// Получение данных сотрудника из базы данных
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    /// <returns>Сотрудник</returns>
    Employee GetEmployee(int employeeId);

    /// <summary>
    /// Получение данных всех сотрудников из базы данных
    /// </summary>
    /// <returns>Сотрудники</returns>
    List<Employee> GetAllEmployees();
    
    /// <summary>
    /// Обновление данных сотрудника в базе данных
    /// </summary>
    /// <param name="employee">Сотрудник</param>
    void UpdateEmployee(Employee employee);

    /// <summary>
    /// Удаление данных сотрудника из базы данных
    /// </summary>
    /// <param name="employeeId">ID сотрудника</param>
    void DeleteEmployee(int employeeId);

    /// <summary>
    /// Назначение смены сотруднику
    /// </summary>
    /// <param name="employeeShift">Смена сотрудника</param>
    /// <returns>ID созданной смены</returns>
    int AssignShift(EmployeeShift employeeShift);
}