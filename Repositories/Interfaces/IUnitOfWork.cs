namespace Narratify.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IArticleRepository Articles { get; }
    ICommentRepository Comments { get; }
    IUserRepository Users { get; }
    Task<int> CompleteAsync();
}