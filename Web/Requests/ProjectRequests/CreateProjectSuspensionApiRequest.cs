using Contracts.ProjectEntities;

namespace Web.Requests.ProjectRequests;

public class CreateProjectSuspensionApiRequest
{
    public required Project Project { get; set; }
    public required DateTimeOffset DateSuspended { get; set; }
}