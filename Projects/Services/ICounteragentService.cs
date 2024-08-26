using Contracts.ProjectEntities;

namespace Projects.Services;

/// <summary>
/// Интерфейс сервиса для работы с контрагентами.
/// </summary>
public interface ICounteragentService
{
    Task<Counteragent> CreateCounteragentAsync(CreateCounteragentRequest counteragent, CancellationToken cancellationToken);
    
    Task<Counteragent> UpdateCounteragentAsync(UpdateCounteragentRequest counteragent, CancellationToken cancellationToken);
    
    Task<Counteragent?> GetCounteragentByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DeleteCounteragentAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Counteragent>> GetAllCounteragentsAsync(CancellationToken cancellationToken);

}

public class UpdateCounteragentRequest
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

public class CreateCounteragentRequest
{
    public required string Name { get; set; }
    public required string Contact { get; set; }
    public required string Phone { get; set; }
    public string? INN { get; set; }
    public string? OGRN { get; set; }
    public string? AccountNumber { get; set; }
    public string? BIK { get; set; }
}