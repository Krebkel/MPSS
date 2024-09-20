using Microsoft.Extensions.Logging;
using Contracts.ProductEntities;
using DataContracts;
using Microsoft.EntityFrameworkCore;

namespace Products.Services;

public class ComponentService : IComponentService
{
    private readonly IRepository<Component> _componentRepository;
    private readonly ILogger<ComponentService> _logger;
    private readonly IValidator<Component> _componentValidator;

    public ComponentService(
        IRepository<Component> componentRepository,
        ILogger<ComponentService> logger,
        IValidator<Component> componentValidator)
    {
        _componentRepository = componentRepository;
        _logger = logger;
        _componentValidator = componentValidator;
    }

    public async Task<Component> CreateComponentAsync(CreateComponentRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var createdComponent = new Component
        {
            Name = request.Name,
            Price = request.Price,
            Weight = request.Weight
        };

        await _componentRepository.AddAsync(createdComponent, cancellationToken);
        _logger.LogInformation("Компонент успешно добавлен: {@Component}", createdComponent);

        return createdComponent;
    }

    public async Task<Component> UpdateComponentAsync(UpdateComponentRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var component = await _componentValidator.ValidateAndGetEntityAsync(request.Id,
            _componentRepository, "Компонент", cancellationToken);

        component.Name = request.Name;
        component.Price = request.Price;
        component.Weight = request.Weight;

        await _componentRepository.UpdateAsync(component, cancellationToken);
        _logger.LogInformation("Компонент успешно обновлен: {@Component}", component);

        return component;
    }

    public async Task<Component?> GetComponentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _componentRepository.GetAll()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteComponentAsync(int id, CancellationToken cancellationToken)
    {
        var component = await _componentRepository.GetAll()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (component == null) return false;

        await _componentRepository.DeleteAsync(component, cancellationToken);
        _logger.LogInformation("Компонент с ID {Id} успешно удален", id);
        return true;
    }

    public async Task<IEnumerable<Component>> GetAllComponentsAsync(CancellationToken cancellationToken)
    {
        return await _componentRepository.GetAll().ToListAsync(cancellationToken);
    }
}