using Narratify.Data;
using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
