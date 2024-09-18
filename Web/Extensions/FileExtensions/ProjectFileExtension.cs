using Files.Services;
using Web.Requests.FileRequests;

namespace Web.Extensions.FileExtensions;

public static class ProjectFileExtensions
{
    public static SaveProjectFileRequest ToSaveProjectFileRequest(this CreateProjectFileApiRequest request)
    {
        return new SaveProjectFileRequest
        {
            File = request.File,
            ProjectId = request.ProjectId
        };
    }
}