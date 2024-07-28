namespace Web.Responses.ProjectResponses;

public class ApiProject
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset DeadlineDate { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? DateSuspended { get; set; }
    public int? CounteragentId { get; set; }
    public required double TotalCost { get; set; }
    public required int ResponsibleEmployeeId { get; set; }
    public required bool IsActive { get; set; }
}