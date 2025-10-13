using Microsoft.EntityFrameworkCore;
using Narratify.Data;
using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await DbContext.Articles.Include(a => a.Author).ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedWithIncludesAsync()
    {
        return await DbContext.Articles
            .Where(a => a.IsPublished)
            .Include(a => a.Author)
            .ToListAsync();
    }
}
