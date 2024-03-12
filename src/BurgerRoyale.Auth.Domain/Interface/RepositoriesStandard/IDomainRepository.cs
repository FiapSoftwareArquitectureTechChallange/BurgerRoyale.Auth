namespace BurgerRoyale.Auth.Domain.Interface.RepositoriesStandard
{
    public interface IDomainRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
