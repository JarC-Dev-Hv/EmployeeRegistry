namespace EmployeeRegistry.API.Repository
{
    public interface ISearchRepository<TEntity, TSearch>
    {
        Task<IEnumerable<TEntity>> Search(TSearch searchParams);
    }
}
