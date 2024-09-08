using System.IO.Compression;
using Contracts.FileEntities;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Files.Services;

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

    public async Task<ProjectFile> SaveFileAsync(IFormFile file, int projectId, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        var project = await _projectValidator.ValidateAndGetEntityAsync(
            projectId,
            _projectRepository, 
            "Проект", 
            ct);
            
        var projectFile = new ProjectFile
        {
            Name = file.FileName,
            UploadDate = DateTimeOffset.UtcNow,
            Project = project
        };

        var fileName = $"{projectFile.UploadDate:yyyy.MM.dd}-{projectId}-{Path.GetFileName(file.FileName)}";
        var relativePath = Path.Combine(ProjectFilesFolder, projectId.ToString(), projectFile.UploadDate.Year.ToString(), projectFile.UploadDate.Month.ToString());
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

    public async Task<Stream> GetFileAsync(int fileId, CancellationToken ct)
    {
        var projectFile = await _projectFileRepository.GetByIdAsync(fileId, ct);
        if (projectFile == null)
            throw new FileNotFoundException("File not found");

        var fullPath = Path.Combine(_basePath, projectFile.FilePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found", fullPath);

        var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return new GZipStream(fileStream, CompressionMode.Decompress);
    }

    public async Task DeleteFileAsync(int fileId, CancellationToken ct)
    {
        var projectFile = await _projectFileRepository.GetByIdAsync(fileId, ct);
        if (projectFile == null)
            return;

        var fullPath = Path.Combine(_basePath, projectFile.FilePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        await _projectFileRepository.DeleteAsync(projectFile, ct);
    }

    public string GetContentType(string fileName)
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