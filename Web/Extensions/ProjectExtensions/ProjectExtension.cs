using Projects.Services;
using Web.Requests.ProjectRequests;

namespace Web.Extensions.ProjectExtensions;

public static class ProjectExtension
{
    internal static CreateProjectRequest ToCreateProjectApiRequest(this CreateProjectApiRequest request)
    {
        return new CreateProjectRequest
        {
            Name = request.Name,
            Address = request.Address,
            DeadlineDate = request.DeadlineDate,
            StartDate = request.StartDate,
            Counteragent = request.Counteragent,
            ResponsibleEmployee = request.ResponsibleEmployee,
            ProjectStatus = request.ProjectStatus,
            Note = request.Note,
            ManagerShare = request.ManagerShare
        };
    }
    
    internal static UpdateProjectRequest ToUpdateProjectApiRequest(this UpdateProjectApiRequest request)
    {
        return new UpdateProjectRequest()
        {
            Id = request.Id,
            Name = request.Name,
            Address = request.Address,
            DeadlineDate = request.DeadlineDate,
            StartDate = request.StartDate,
            Counteragent = request.Counteragent,
            ResponsibleEmployee = request.ResponsibleEmployee,
            ProjectStatus = request.ProjectStatus,
            Note = request.Note,
            ManagerShare = request.ManagerShare
        };
    }
}