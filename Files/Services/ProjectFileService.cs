using System.IO.Compression;
using Contracts.FileEntities;
using Contracts.ProjectEntities;
using DataContracts;
using Files.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class ProjectFileService : IProjectFileService
{
    private readonly string _basePath;
    private readonly IRepository<ProjectFile> _projectFileRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IValidator<Project> _projectValidator;
    private const string ProjectFilesFolder = "ProjectFiles";

    public ProjectFileService(
        IConfiguration configuration, 
        IRepository<ProjectFile> projectFileRepository,
        IValidator<Project> projectValidator,
        IRepository<Project> projectRepository)
    {
        _basePath = configuration.GetValue<string>("FilesBasePath") ?? Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
        _projectFileRepository = projectFileRepository;
        _projectRepository = projectRepository;
        _projectValidator = projectValidator;
        EnsureDirectoriesExist();
    }

    private void EnsureDirectoriesExist()
    {
        Directory.CreateDirectory(Path.Combine(_basePath, ProjectFilesFolder));
    }

    public async Task<ProjectFile> SaveFileAsync(SaveProjectFileRequest request, CancellationToken ct)
    {
        var file = request.File;
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            request.ProjectId,
            _projectRepository, 
            "Проект", 
            ct);

        var projectFile = new ProjectFile
        {
            Name = file.FileName,
            UploadDate = DateTimeOffset.UtcNow,
            Project = project
        };

        var fileName = $"{projectFile.UploadDate:yyyy.MM.dd}-{request.ProjectId}-{Path.GetFileName(file.FileName)}";
        var relativePath = Path.Combine(ProjectFilesFolder, request.ProjectId.ToString(), projectFile.UploadDate.Year.ToString(), projectFile.UploadDate.Month.ToString());
        var fullPath = Path.Combine(_basePath, relativePath);
        Directory.CreateDirectory(fullPath);

        var filePath = Path.Combine(fullPath, fileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        await using (var compressStream = new GZipStream(fileStream, CompressionMode.Compress))
        {
            await file.CopyToAsync(compressStream, ct);
        }

        projectFile.FilePath = Path.Combine(relativePath, fileName);
        await _projectFileRepository.AddAsync(projectFile, ct);

        return projectFile;
    }

    public async Task<ProjectFile> GetProjectFileByIdAsync(int id, CancellationToken ct)
    {
        return await _projectFileRepository.GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteFileAsync(int id, CancellationToken ct)
    {
        var projectFile = await _projectFileRepository.GetByIdAsync(id, ct);
        if (projectFile == null)
            return false;

        var fullPath = Path.Combine(_basePath, projectFile.FilePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        await _projectFileRepository.DeleteAsync(projectFile, ct);
        return true;
    }

    public async Task<IEnumerable<object>> GetProjectFilesByProjectIdAsync(int projectId, CancellationToken ct)
    {
        return await _projectFileRepository
            .GetAll()
            .Where(pf => pf.Project.Id == projectId)
            .Select(pf => new
            {
                Id = pf.Id,
                FileName = pf.Name,
                FilePath = pf.FilePath,
                ProjectId = pf.Project.Id
            })
            .ToListAsync(ct);
    }

    public async Task<(Stream fileStream, string fileName, string contentType)> GetFileForDownloadAsync(int id, CancellationToken ct)
    {
        var projectFile = await _projectFileRepository.GetByIdAsync(id, ct);
        if (projectFile == null)
            throw new FileNotFoundException("File not found");

        var fullPath = Path.Combine(_basePath, projectFile.FilePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found", fullPath);

        var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        var decompressedStream = new GZipStream(fileStream, CompressionMode.Decompress);
        
        return (decompressedStream, projectFile.Name, GetContentType(projectFile.Name));
    }

    public async Task<(Stream fileStream, string contentType)> GetFileForViewAsync(int id, CancellationToken ct)
    {
        var (fileStream, _, contentType) = await GetFileForDownloadAsync(id, ct);
        return (fileStream, contentType);
    }

    private string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream",
        };
    }
}