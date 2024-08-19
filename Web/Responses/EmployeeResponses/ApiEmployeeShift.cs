namespace Web.Responses.EmployeeResponses;

public class ApiEmployeeShift
{
    public required int Id { get; set; }
    public required int ProjectId { get; set; }
    public required int EmployeeId { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? HoursWorked { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
}