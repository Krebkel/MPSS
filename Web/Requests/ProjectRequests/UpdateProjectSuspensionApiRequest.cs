namespace Web.Requests.ProjectRequests;

public class UpdateProjectSuspensionApiRequest
{
    public required int Id { get; set; }
    public required int Project { get; set; }
    public required DateTimeOffset DateSuspended { get; set; }
}