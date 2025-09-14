
namespace Narratify.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArticleRepository Articles { get; }
    ICategoryRepository Categories { get; }
    ICommentRepository Comments { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}
