using Contracts.EmployeeEntities;

namespace Web.Services.EmployeeServices;

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
    float CalculateTotalTime(DateTimeOffset arrival, DateTimeOffset departure);
}