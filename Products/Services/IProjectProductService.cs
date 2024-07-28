using Contracts.ProductEntities;

namespace Products.Services;

public interface IProjectProductService
{
    /// <summary>
    /// Добавление изделия в проекте в базу данных
    /// </summary>
    /// <param name="projectProduct">Изделие в проекте</param>
    /// <returns>ID созданного изделия в проекте</returns>
    public int CreateProjectProduct(ProjectProduct projectProduct);

    /// <summary>
    /// Обновление изделия в проекте в базе данных
    /// </summary>
    /// <param name="projectProduct">Изделие в проекте</param>
    public void UpdateProjectProduct(ProjectProduct projectProduct);

    /// <summary>
    /// Добавление изделия в проекте в базу данных
    /// </summary>
    /// <param name="projectProductId">ID изделия в проекте</param>
    public void DeleteProjectProduct(int projectProductId);

    /// <summary>
    /// Получение изделия в проекте из базы данных
    /// </summary>
    /// <param name="projectProductId">ID изделия в проекте</param>
    public ProjectProduct GetProjectProduct(int projectProductId);
}