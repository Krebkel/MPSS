using Contracts;

namespace Web.Requests.ProjectRequests;

public class CreateProjectApiRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public int? Counteragent { get; set; }
    public required int ResponsibleEmployee { get; set; }
    public required ProjectStatus ProjectStatus { get; set; }
    public required float ManagerShare { get; set; }
}