using Files.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts.FileEntities;
using Microsoft.Extensions.Logging;
using Web.Extensions.FileExtensions;
using Web.Requests.FileRequests;

namespace Web.Controllers.Base.Files;

[Authorize]
[ApiController]
[Route("api/projectFiles")]
public class ProjectFileController : ControllerBase
{
    private readonly IProjectFileService _projectFileService;
    private readonly ILogger<ProjectFileController> _logger;

    public ProjectFileController(IProjectFileService projectFileService, ILogger<ProjectFileController> logger)
    {
        _projectFileService = projectFileService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectFile))]
    public async Task<IActionResult> AddProjectFile(
        [FromForm] CreateProjectFileApiRequest request, CancellationToken ct)
    {
        try
        {
            var saveRequest = request.ToSaveProjectFileRequest();
            var projectFile = await _projectFileService.SaveFileAsync(saveRequest, ct);

            _logger.LogInformation("Файл {@FileName} успешно добавлен в проект {@ProjectId}", projectFile.Name, request.ProjectId);

            return Ok(projectFile);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Ошибка при добавлении файла: некорректный файл");
            return BadRequest("Неправильный формат файла");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении файла в проект");
            return BadRequest($"Ошибка добавления файла в проект: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProjectFile))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectFile(int id, CancellationToken ct)
    {
        try
        {
            var projectFile = await _projectFileService.GetProjectFileByIdAsync(id, ct);
            if (projectFile == null)
            {
                _logger.LogWarning("Проект с ID {Id} не найден", id);
                return NotFound($"Проект с ID {id} не найден");
            }

            return Ok(projectFile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения файлов проекта");
            return BadRequest($"Ошибка получения файлов проекта: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectFile(int id, CancellationToken ct)
    {
        try
        {
            var result = await _projectFileService.DeleteFileAsync(id, ct);
            if (!result)
            {
                _logger.LogWarning("Файл с ID {Id} не найден", id);
                return NotFound($"Файл с ID {id} не найден");
            }

            _logger.LogInformation("Файл с ID {Id} успешно удален", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка удаления файла проекта");
            return BadRequest($"Ошибка удаления файла проекта: {ex.Message}");
        }
    }

    [HttpGet("byProject/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectFile>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectFilesByProjectId(int projectId, CancellationToken ct)
    {
        try
        {
            var projectFiles = await _projectFileService.GetProjectFilesByProjectIdAsync(projectId, ct);
        
            if (projectFiles == null)
            {
                _logger.LogWarning("Проект с ID {ProjectId} не найден", projectId);
                return NotFound($"Проект с ID {projectId} не найден");
            }

            return Ok(projectFiles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения файлов проекта");
            return BadRequest($"Ошибка получения файлов проекта: {ex.Message}");
        }
    }

    [HttpGet("download/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadProjectFile(int id, CancellationToken ct)
    {
        try
        {
            var (fileStream, fileName, contentType) = await _projectFileService.GetFileForDownloadAsync(id, ct);
            return File(fileStream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            _logger.LogWarning("Сохраненный файл по ID {Id} не найден", id);
            return NotFound($"Сохраненный файл по ID {id} не найден");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка скачивания файла проекта");
            return BadRequest($"Ошибка скачивания файла проекта: {ex.Message}");
        }
    }

    [HttpGet("view/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ViewProjectFile(int id, CancellationToken ct)
    {
        try
        {
            var (fileStream, contentType) = await _projectFileService.GetFileForViewAsync(id, ct);
            return File(fileStream, contentType);
        }
        catch (FileNotFoundException)
        {
            _logger.LogWarning("Сохраненный файл по ID {Id} не найден", id);
            return NotFound($"Сохраненный файл по ID {id} не найден");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения файла");
            return BadRequest($"Ошибка получения файла: {ex.Message}");
        }
    }
}