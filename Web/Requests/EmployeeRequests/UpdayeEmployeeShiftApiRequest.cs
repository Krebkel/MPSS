namespace Web.Requests.EmployeeRequests;

public class UpdateEmployeeShiftApiRequest
{
    public required int ProjectId { get; set; }
    public required int EmployeeId { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public double? Wage { get; set; }
}