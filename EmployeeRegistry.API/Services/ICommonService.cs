namespace EmployeeRegistry.API.Services
{
    public interface ICommonService<TDto, TInsertDto, TUpdateDto>
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto> GetById(int id);
        Task<TDto> Add(TInsertDto beerInsertDto);
        Task<TDto> Update(int id, TUpdateDto beerUpdateDto);
        Task<TDto> Delete(int id);
    }
}
