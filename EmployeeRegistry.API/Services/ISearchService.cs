namespace EmployeeRegistry.API.Services
{
    public interface ISearchService<TDto, TSearch>
    {
        Task<IEnumerable<TDto>> Search(TSearch searchParams);
    }
}
