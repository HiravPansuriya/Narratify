using Narratify.Data;
using Narratify.Models.Entities;
using Narratify.Repositories.Interfaces;

namespace Narratify.Repositories.Implementations;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
