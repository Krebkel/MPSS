using Contracts.ProjectEntities;

namespace Web.Requests.ProjectRequests;

public class CreateProjectApiRequest
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? DateSuspended { get; set; }
    public int? CounteragentId { get; set; }
    public required double TotalCost { get; set; }
    public required int ResponsibleEmployeeId { get; set; }
    public required ProjectStatus ProjectStatus { get; set; }
    public required float ManagerShare { get; set; }
}