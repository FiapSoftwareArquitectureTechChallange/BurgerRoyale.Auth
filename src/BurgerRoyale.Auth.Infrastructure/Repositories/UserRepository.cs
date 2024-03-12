using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.Infrastructure.Context;
using BurgerRoyale.Auth.Infrastructure.RepositoriesStandard;

namespace BurgerRoyale.Auth.Infrastructure.Repositories
{
    public class UserRepository : DomainRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
