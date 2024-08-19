using Contracts.ProjectEntities;

namespace Projects.Services;

/// <summary>
/// Интерфейс сервиса для работы с проектами.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Добавление проекта в базу данных
    /// </summary>
    /// <param name="project">Проект</param>
    /// <returns>ID созданного проекта</returns>
    int CreateProject(Project project);

    /// <summary>
    /// Получение данных проекта из базы данных
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    /// <returns>Проект</returns>
    Project GetProject(int projectId);

    /// <summary>
    /// Получение данных всех проектов из базы данных
    /// </summary>
    /// <returns>Список проектов</returns>
    List<Project> GetAllProjects();

    /// <summary>
    /// Обновление данных проекта в базе данных
    /// </summary>
    /// <param name="project">Проект</param>
    void UpdateProject(Project project);

    /// <summary>
    /// Удаление проекта из базы данных
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    void DeleteProject(int projectId);

    /// <summary>
    /// Расчет совокупной стоимости проекта
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    /// <returns>Суммарная стоимость проекта</returns>
    double CalculateTotalCost(int projectId);

    /// <summary>
    /// Расчет средней производительности по проекту
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    /// <returns>Средняя производительность</returns>
    double CalculateAverageProductivity(int projectId);

    /// <summary>
    /// Изменение статуса проекта
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    void ChangeProjectStatus(int projectId, ProjectStatus projectStatus);

    /// <summary>
    /// Распределение зарплат по сотрудникам
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="managerShare"></param>
    void DistributeProjectBonus(int projectId, double managerShare);

    /// <summary>
    /// Расчет заработной платы сотрудников за выполненные проекты
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    double CalculateTotalWageForDoneProjects(int employeeId);
}