using Contracts.ProjectEntities;
using Data;

namespace Projects.Services;

/// <summary>
/// Сервис для работы с контрагентами
/// </summary>
public class CounteragentService : ICounteragentService
{
    private readonly AppDbContext _context;

    public CounteragentService(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public int CreateCounteragent(Counteragent counteragent)
    {
        ValidateCounteragent(counteragent);
        
        _context.Counteragents.Add(counteragent);
        _context.SaveChanges();
        return counteragent.Id;
    }

    /// <inheritdoc />
    public Counteragent GetCounteragent(int counteragentId)
    {
        return _context.Counteragents.Find(counteragentId);
    }
    
    /// <inheritdoc />
    public List<Counteragent> GetAllCounteragents()
    {
        return _context.Counteragents.OrderBy(c=>c.Name).ToList();
    }
    
    /// <inheritdoc />
    public void UpdateCounteragent(Counteragent counteragent)
    {
        ValidateCounteragent(counteragent);
        
        var existingCounteragent = _context.Counteragents.Find(counteragent.Id);
    
        if (existingCounteragent != null)
        {
            existingCounteragent.Name = counteragent.Name ?? existingCounteragent.Name;
            existingCounteragent.Contact = counteragent.Contact ?? existingCounteragent.Contact;
            existingCounteragent.Phone = counteragent.Phone ?? existingCounteragent.Phone;
            existingCounteragent.INN = counteragent.INN ?? existingCounteragent.INN;
            existingCounteragent.OGRN = counteragent.OGRN ?? existingCounteragent.OGRN;
            existingCounteragent.AccountNumber = counteragent.AccountNumber ?? existingCounteragent.AccountNumber;
            existingCounteragent.BIK = counteragent.BIK ?? existingCounteragent.BIK;

            _context.SaveChanges();
        }
    }

    /// <inheritdoc />
    public void DeleteCounteragent(int counteragentId)
    {
        var counteragent = _context.Counteragents.Find(counteragentId);
        if (counteragent != null)
        {
            _context.Counteragents.Remove(counteragent);
            _context.SaveChanges();
        }
    }
    
    private void ValidateCounteragent(Counteragent counteragent)
    {
        if (string.IsNullOrWhiteSpace(counteragent.Name))
        {
            throw new ArgumentException("Ошибка в наименовании контрагента");
        }

        if (string.IsNullOrWhiteSpace(counteragent.Contact))
        {
            throw new ArgumentException("Ошибка в контактном лице контрагента.");
        }

        if (string.IsNullOrWhiteSpace(counteragent.Phone))
        {
            throw new ArgumentException("Некорректно введен телефон контактного лица.");
        }
    }
}