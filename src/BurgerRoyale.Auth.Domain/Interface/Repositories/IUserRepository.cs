using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Interface.RepositoriesStandard;

namespace BurgerRoyale.Auth.Domain.Interface.Repositories
{
    public interface IUserRepository : IDomainRepository<User>
    {
    }
}
