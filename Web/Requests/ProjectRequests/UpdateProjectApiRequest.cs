using Contracts;
using Contracts.EmployeeEntities;
using Contracts.ProjectEntities;

namespace Web.Requests.ProjectRequests;

public class UpdateProjectApiRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? DateSuspended { get; set; }
    public Counteragent? Counteragent { get; set; }
    public required Employee ResponsibleEmployee { get; set; }
    public required ProjectStatus ProjectStatus { get; set; }
    public required float ManagerShare { get; set; }
}