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

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await DbContext.Set<T>().Where(expression).ToListAsync();
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
}