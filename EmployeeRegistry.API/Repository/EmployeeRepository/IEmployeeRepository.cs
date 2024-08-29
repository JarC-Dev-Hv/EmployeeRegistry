using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Models;

namespace EmployeeRegistry.API.Repository.EmployeeRepository
{
    public interface IEmployeeRepository: IRepository<Employee>, ISearchRepository<Employee, EmployeeSearchDto>
    {
    }
}
