using Microsoft.Extensions.Logging;
using Contracts.ProjectEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Projects.Services;

/// <summary>
/// Сервис для работы с контрагентами
/// </summary>
public class CounteragentService : ICounteragentService
{
    private readonly IRepository<Counteragent> _counteragentRepository;
    private readonly ILogger<CounteragentService> _logger;
    private readonly IValidator<Counteragent> _counteragentValidator;

    public CounteragentService(
        IRepository<Counteragent> counteragentRepository,
        ILogger<CounteragentService> logger,
        IValidator<Counteragent> counteragentValidator)
    {
        _counteragentRepository = counteragentRepository;
        _logger = logger;
        _counteragentValidator = counteragentValidator;
    }


    public async Task<Counteragent> CreateCounteragentAsync(CreateCounteragentRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        
        var createdCounteragent = new Counteragent
        {
            Name = request.Name,
            Contact = request.Contact,
            Phone = request.Phone,
            INN = request.INN,
            OGRN = request.OGRN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };
        
        await _counteragentRepository.AddAsync(createdCounteragent, cancellationToken);

        _logger.LogInformation("Контрагент успешно добавлен: {@Counteragent}", createdCounteragent);
        return createdCounteragent;
    }
    
    public async Task<Counteragent> UpdateCounteragentAsync(
        UpdateCounteragentRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var counteragent = await _counteragentValidator.ValidateAndGetEntityAsync(
            request.Id, _counteragentRepository, "Контрагент", cancellationToken);

        counteragent.Id = request.Id;
        counteragent.Name = request.Name;
        counteragent.Contact = request.Contact;
        counteragent.Phone = request.Phone;
        counteragent.INN = request.INN;
        counteragent.OGRN = request.OGRN;
        counteragent.AccountNumber = request.AccountNumber;
        counteragent.BIK = request.BIK;

        await _counteragentRepository.UpdateAsync(counteragent, cancellationToken);

        _logger.LogInformation("Контрагент успешно обновлен: {@Counteragent}", counteragent);
        return counteragent;
    }
    
    public async Task<Counteragent?> GetCounteragentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _counteragentRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteCounteragentAsync(int id, CancellationToken cancellationToken)
    {
        var counteragent = await _counteragentRepository
            .GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (counteragent == null) return false;

        await _counteragentRepository.DeleteAsync(counteragent, cancellationToken);
        _logger.LogInformation("Контрагент с ID {Id} успешно удален", id);
        return true;
    }
    
    public async Task<IEnumerable<Counteragent>> GetAllCounteragentsAsync(CancellationToken cancellationToken)
    {
        return await _counteragentRepository.GetAll().ToListAsync(cancellationToken);
    }
}