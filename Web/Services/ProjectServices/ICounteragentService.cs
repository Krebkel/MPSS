using Contracts.ProjectEntities;

namespace Web.Services.ProjectServices;

/// <summary>
/// Интерфейс сервиса для работы с контрагентами.
/// </summary>
public interface ICounteragentService
{
    /// <summary>
    /// Добавление контрагента в базу данных
    /// </summary>
    /// <param name="counteragent">Контрагент</param>
    /// <returns>ID созданного контрагента</returns>
    int CreateCounteragent(Counteragent counteragent);

    /// <summary>
    /// Обновление данных контрагента в базе данных
    /// </summary>
    /// <param name="counteragent">Контрагент</param>
    void UpdateCounteragent(Counteragent counteragent);

    /// <summary>
    /// Удаление контрагента из базы данных
    /// </summary>
    /// <param name="counteragentId">ID контрагента</param>
    void DeleteCounteragent(int counteragentId);
}