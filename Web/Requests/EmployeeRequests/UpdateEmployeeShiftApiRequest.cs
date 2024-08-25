using Contracts.EmployeeEntities;
using Contracts.ProjectEntities;

namespace Web.Requests.EmployeeRequests;

public class UpdateEmployeeShiftApiRequest
{
    public required int Id { get; set; }
    public required Project Project { get; set; }
    public required Employee Employee { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public int? ISN { get; set; }
}