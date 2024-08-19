namespace Web.Requests.ProjectRequests;

public class UpdateCounteragentApiRequest
{
    public required string Name { get; set; }
    public required string? Contact { get; set; }
    public required string? Phone { get; set; }
    public uint? INN { get; set; }
    public uint? OGRN { get; set; }
    public ulong? AccountNumber { get; set; }
    public uint? BIK { get; set; }
}