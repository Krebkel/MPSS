using Projects.Services;
using Web.Requests.ProjectRequests;

namespace Web.Extensions.ProjectExtensions;

public static class ProjectSuspensionExtension
{
    internal static CreateProjectSuspensionRequest ToCreateProjectSuspensionApiRequest(this CreateProjectSuspensionApiRequest request)
    {
        return new CreateProjectSuspensionRequest
        {
            Project = request.Project,
            DateSuspended = request.DateSuspended
        };
    }
    
    internal static UpdateProjectSuspensionRequest ToUpdateProjectSuspensionApiRequest(this UpdateProjectSuspensionApiRequest request)
    {
        return new UpdateProjectSuspensionRequest()
        {
            Id = request.Id,
            Project = request.Project,
            DateSuspended = request.DateSuspended
        };
    }
}