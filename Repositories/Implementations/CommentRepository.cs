using Narratify.Data;
using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
