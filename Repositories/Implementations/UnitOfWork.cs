using Narratify.Data;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IArticleRepository? _articles;
    private ICommentRepository? _comments;
    private IUserRepository? _users;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IArticleRepository Articles => _articles ??= new ArticleRepository(_context);
    public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}