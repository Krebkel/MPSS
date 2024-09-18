using Contracts.FileEntities;
using Microsoft.AspNetCore.Http;

namespace Files.Services;

public interface IProjectFileService
{
    Task<ProjectFile> SaveFileAsync(SaveProjectFileRequest request, CancellationToken ct);
    Task<ProjectFile> GetProjectFileByIdAsync(int id, CancellationToken ct);
    Task<bool> DeleteFileAsync(int id, CancellationToken ct);
    Task<IEnumerable<object>> GetProjectFilesByProjectIdAsync(int projectId, CancellationToken ct);
    Task<(Stream fileStream, string fileName, string contentType)> GetFileForDownloadAsync(int id, CancellationToken ct);
    Task<(Stream fileStream, string contentType)> GetFileForViewAsync(int id, CancellationToken ct);
}

public class SaveProjectFileRequest
{
    public required IFormFile File { get; set; }
    public required int ProjectId { get; set; }
}