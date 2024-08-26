namespace DataContracts;

public interface IValidator<TEntity> where TEntity : DatabaseEntity
{
    Task ValidateEntityAsync(int? id, IRepository<TEntity> repository, string entityName, CancellationToken cancellationToken);
    
    Task<TEntity> ValidateAndGetEntityAsync(int? id, IRepository<TEntity> repository, string entityName, CancellationToken cancellationToken);
}