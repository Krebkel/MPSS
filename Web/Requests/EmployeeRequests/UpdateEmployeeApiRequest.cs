namespace Web.Requests.EmployeeRequests;

public class UpdateEmployeeApiRequest
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public ulong? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public ulong? INN { get; set; }
    public ulong? AccountNumber { get; set; }
    public ulong? BIK { get; set; }
}