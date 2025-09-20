using System.Linq.Expressions;

namespace Narratify.Repositories.Interfaces;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}