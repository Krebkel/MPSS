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
        _context.Counteragents.Add(counteragent);
        _context.SaveChanges();
        return counteragent.Id;
    }

    /// <inheritdoc />
    public void UpdateCounteragent(Counteragent counteragent)
    {
        _context.Counteragents.Update(counteragent);
        _context.SaveChanges();
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
}