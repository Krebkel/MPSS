using Files.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Base.Files;

[Authorize]
[ApiController]
[Route("api/files")]
public class ProjectFileController : ControllerBase
{
    private readonly IProjectFileService _projectFileService;

    public ProjectFileController(IProjectFileService projectFileService)
    {
        _projectFileService = projectFileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] int projectId, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file");

        try
        {
            var projectFile = await _projectFileService.SaveFileAsync(file, projectId, cancellationToken);
            return Ok(new { Id = projectFile.Id, Name = projectFile.Name, FilePath = projectFile.FilePath });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(int id, CancellationToken cancellationToken)
    {
        try
        {
            var fileStream = await _projectFileService.GetFileAsync(id, cancellationToken);
            
            string fileName = $"file_{id}";
            
            var contentType = _projectFileService.GetContentType(fileName);
            return File(fileStream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _projectFileService.DeleteFileAsync(id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("view/{id}")]
    public async Task<IActionResult> ViewFile(int id, CancellationToken cancellationToken)
    {
        try
        {
            var fileStream = await _projectFileService.GetFileAsync(id, cancellationToken);
            
            string fileName = $"file_{id}";
            
            var contentType = _projectFileService.GetContentType(fileName);
            return File(fileStream, contentType);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}