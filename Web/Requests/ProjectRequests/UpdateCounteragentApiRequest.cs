namespace Web.Requests.ProjectRequests;

public class UpdateCounteragentApiRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string? Contact { get; set; }
    public required string? Phone { get; set; }
    public string? INN { get; set; }
    public string? OGRN { get; set; }
    public string? AccountNumber { get; set; }
    public string? BIK { get; set; }
}