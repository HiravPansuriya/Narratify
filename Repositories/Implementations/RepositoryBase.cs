using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Narratify.Data;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly ApplicationDbContext DbContext;

    protected RepositoryBase(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var list = await DbContext.Set<T>().ToListAsync();
        return list;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        T? value = await DbContext.Set<T>().FindAsync(id);
        return value;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await DbContext.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbContext.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.Where(expression).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        DbContext.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }

    public async Task<int> CountAsync()
    {
        return await DbContext.Set<T>().CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await DbContext.Set<T>().CountAsync(expression);
    }
}