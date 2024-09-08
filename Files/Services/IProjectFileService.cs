using Contracts.FileEntities;
using Microsoft.AspNetCore.Http;

namespace Files.Services;

public interface IProjectFileService
{
    Task<ProjectFile> SaveFileAsync(IFormFile file, int projectId, CancellationToken ct);
    Task<Stream> GetFileAsync(int fileId, CancellationToken ct);
    Task DeleteFileAsync(int fileId, CancellationToken ct);
    string GetContentType(string fileName);
}