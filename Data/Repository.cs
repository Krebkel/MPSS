using System.Linq.Expressions;
using DataContracts;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Data;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : DatabaseEntity
{
    private readonly AppDbContext _dbContext;
    
    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>().AsQueryable();

    public async Task<RepositoryAddResult> AddAsync(TEntity job, CancellationToken ct)
    {
        await _dbContext.Set<TEntity>().AddAsync(job, ct);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch(DbUpdateException e) 
            when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            return RepositoryAddResult.AlreadyExists;
        }

        return RepositoryAddResult.Added;
    }

    public async Task<RepositoryUpdateResult> UpdateAsync(TEntity entity, CancellationToken ct)
    {
        _dbContext.Set<TEntity>().Update(entity);
        
        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            await ReloadEntity(entity, ct);
            return RepositoryUpdateResult.ConcurrencyError;
        }

        return RepositoryUpdateResult.Updated;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken ct)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(int? id, CancellationToken ct)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    private async Task ReloadEntity(TEntity entity, CancellationToken ct)
    {
        var entry = _dbContext.Entry(entity);
        await entry.ReloadAsync(ct);
    }
}