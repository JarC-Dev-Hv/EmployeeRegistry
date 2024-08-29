using AutoMapper;
using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Models;

namespace EmployeeRegistry.API.Automappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de Employee a EmployeeDto y viceversa
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            // Mapeo de EmployeeInsertDto a Employee
            CreateMap<EmployeeInsertDto, Employee>();

            // Mapeo de EmployeeUpdateDto a Employee
            CreateMap<EmployeeUpdateDto, Employee>();
        }
    }
}
