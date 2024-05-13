using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.Infrastructure.Context;
using BurgerRoyale.Auth.Infrastructure.RepositoriesStandard;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Auth.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class UserRepository(ApplicationDbContext applicationDbContext) : DomainRepository<User>(applicationDbContext), IUserRepository
{
}
