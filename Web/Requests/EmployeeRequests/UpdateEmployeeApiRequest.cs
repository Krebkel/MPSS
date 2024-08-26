namespace Web.Requests.EmployeeRequests;

public class UpdateEmployeeApiRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public string? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string? INN { get; set; }
    public string? AccountNumber { get; set; }
    public string? BIK { get; set; }
}