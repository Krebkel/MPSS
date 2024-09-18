using Microsoft.AspNetCore.Http;

namespace Web.Requests.FileRequests;

public class CreateProjectFileApiRequest
{
    public required IFormFile File { get; set; }
    public required int ProjectId { get; set; }
}