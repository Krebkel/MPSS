namespace Web.Requests.ProjectRequests;

public class CreateProjectSuspensionApiRequest
{
    public required int Project { get; set; }
    public required DateTimeOffset DateSuspended { get; set; }
}