using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// Diyelim ki GenericRepository'de Transaction modeline bağlı bir Entity türü kullanıyoruz.

public interface IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filter);
    // Diğer işlevler...
}

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Where(filter).ToList();
    }

    // Diğer işlevler...
}
