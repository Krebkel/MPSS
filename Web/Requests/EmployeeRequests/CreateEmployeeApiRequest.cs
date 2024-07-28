namespace Web.Requests.EmployeeRequests;

public class CreateEmployeeApiRequest
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public bool IsDriver { get; set; }
    public uint? Passport { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public uint? INN { get; set; }
    public ulong? AccountNumber { get; set; }
    public uint? BIK { get; set; }
}