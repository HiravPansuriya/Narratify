using Narratify.Data;
using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
