using Contracts;
using Contracts.ProjectEntities;

namespace Projects.Services;

/// <summary>
/// Интерфейс сервиса для работы с приостановлением проектов.
/// </summary>
public interface IProjectSuspensionService
{
    Task<ProjectSuspension> CreateProjectSuspensionAsync(CreateProjectSuspensionRequest projectSuspension, CancellationToken cancellationToken);
    
    Task<ProjectSuspension> UpdateProjectSuspensionAsync(UpdateProjectSuspensionRequest projectSuspension, CancellationToken cancellationToken);
    
    Task<ProjectSuspension?> GetProjectSuspensionByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteProjectSuspensionAsync(int id, CancellationToken cancellationToken);
}

public class UpdateProjectSuspensionRequest
{
    public required int Id { get; set; }
    public required Project Project { get; set; }
    public required DateTimeOffset DateSuspended { get; set; }
}

public class CreateProjectSuspensionRequest
{
    public required Project Project { get; set; }
    public required DateTimeOffset DateSuspended { get; set; }
}