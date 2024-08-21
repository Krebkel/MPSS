using Contracts.ProjectEntities;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Extensions.ProjectExtensions;

public static class ProjectExtension
{
    public static Project ToProject(this CreateProjectApiRequest apiRequest)
    {
        return new Project
        {
            Name = apiRequest.Name,
            Address = apiRequest.Address,
            DeadlineDate = apiRequest.DeadlineDate,
            StartDate = apiRequest.StartDate,
            DateSuspended = apiRequest.DateSuspended,
            CounteragentId = apiRequest.CounteragentId,
            ResponsibleEmployeeId = apiRequest.ResponsibleEmployeeId,
            ProjectStatus = apiRequest.ProjectStatus,
            ManagerShare = apiRequest.ManagerShare
        };
    }

    public static Project ToProject(this UpdateProjectApiRequest apiRequest, int id)
    {
        return new Project
        {
            Id = id,
            Name = apiRequest.Name,
            Address = apiRequest.Address,
            DeadlineDate = apiRequest.DeadlineDate,
            StartDate = apiRequest.StartDate,
            DateSuspended = apiRequest.DateSuspended,
            CounteragentId = apiRequest.CounteragentId,
            ResponsibleEmployeeId = apiRequest.ResponsibleEmployeeId,
            ProjectStatus = apiRequest.ProjectStatus,
            ManagerShare = apiRequest.ManagerShare
        };
    }

    public static ApiProject ToApiProject(this Project project)
    {
        return new ApiProject
        {
            Id = project.Id,
            Name = project.Name,
            Address = project.Address,
            DeadlineDate = project.DeadlineDate,
            StartDate = project.StartDate,
            DateSuspended = project.DateSuspended,
            CounteragentId = project.CounteragentId,
            ResponsibleEmployeeId = project.ResponsibleEmployeeId,
            ProjectStatus = project.ProjectStatus,
            ManagerShare = project.ManagerShare
        };
    }
}