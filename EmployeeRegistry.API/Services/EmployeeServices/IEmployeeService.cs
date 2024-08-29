using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Services.EmployeeServices
{
    public interface IEmployeeService : ICommonService<EmployeeDto, EmployeeInsertDto, EmployeeUpdateDto>, ISearchService<EmployeeDto, EmployeeSearchDto>
    {
        // Métodos específicos de EmployeeService
        
    }
}
