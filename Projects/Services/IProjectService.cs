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
    /// Приостановка выполнения проекта и заморозка времени до дедлайна
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    void SuspendProject(int projectId);

    /// <summary>
    /// Продолжение выполнения проекта и пересчет дедлайна
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    void ContinueProject(int projectId);
}