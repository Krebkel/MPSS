using Contracts.ProjectEntities;

namespace Projects.Services;

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
    /// Получение данных контрагента из базы данных
    /// </summary>
    /// <param name="counteragentId">ID контрагента</param>
    /// <returns>Контрагент</returns>
    Counteragent GetCounteragent(int counteragentId);

    /// <summary>
    /// Получение данных всех контрагентов из базы данных
    /// </summary>
    /// <returns>Список контрагентов</returns>
    List<Counteragent> GetAllCounteragents();
    
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