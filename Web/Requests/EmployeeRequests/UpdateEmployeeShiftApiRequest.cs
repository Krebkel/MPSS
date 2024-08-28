namespace Web.Requests.EmployeeRequests;

public class UpdateEmployeeShiftApiRequest
{
    public required int Id { get; set; }
    public required int Project { get; set; }
    public required int Employee { get; set; }
    public required DateTimeOffset Date { get; set; }
    public DateTimeOffset? Arrival { get; set; }
    public DateTimeOffset? Departure { get; set; }
    public float? TravelTime { get; set; }
    public bool ConsiderTravel { get; set; }
    public int? ISN { get; set; }
}