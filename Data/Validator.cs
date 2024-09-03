using DataContracts;
using Microsoft.Extensions.Logging;

namespace Data;

public class Validator<TEntity> : IValidator<TEntity> where TEntity : DatabaseEntity
{
    protected readonly ILogger<Validator<TEntity>> _logger;

    public Validator(ILogger<Validator<TEntity>> logger)
    {
        _logger = logger;
    }

    public async Task ValidateEntityAsync(int? id, IRepository<TEntity> repository, string entityName, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new ApplicationException($"Сущность \"{entityName}\" с ID {id} не найдена.");
    }

    public async Task<TEntity> ValidateAndGetEntityAsync(int? id, IRepository<TEntity> repository, string entityName, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            throw new ArgumentNullException(nameof(id), $"ID для сущности \"{entityName}\" не может быть пустым.");
    
        var entity = await repository.GetByIdAsync(id.Value, cancellationToken);
        if (entity == null)
            throw new ApplicationException($"Сущность \"{entityName}\" с ID {id} не найдена.");
        return entity;
    }
}