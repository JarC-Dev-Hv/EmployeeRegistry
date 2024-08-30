using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Models;
using EmployeeRegistry.API.Utilities;
using EmployeeRegistry.API.Repository.EmployeeRepository;

namespace EmployeeRegistry.API.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeInsertDto> _employeeInsertValidator;
        private readonly IValidator<EmployeeUpdateDto> _employeeUpdateValidator;
        private readonly IValidator<EmployeeSearchDto> _employeeSearchValidator;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IValidator<EmployeeInsertDto> employeeInsertValidator,
            IValidator<EmployeeUpdateDto> employeeUpdateValidator,
            IValidator<EmployeeSearchDto> employeeSearchValidator,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _employeeInsertValidator = employeeInsertValidator;
            _employeeUpdateValidator = employeeUpdateValidator;
            _employeeSearchValidator = employeeSearchValidator;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            try
            {
                var employees = await _employeeRepository.GetAll();
                return employees.Select(b => _mapper.Map<EmployeeDto>(b));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employees.");
                throw new ServiceException("An error occurred while retrieving employees.", ex);
            }
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetById(id);
                return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the employee.");
                throw new ServiceException("An error occurred while retrieving the employee.", ex);
            }
        }

        public async Task<EmployeeDto> Add(EmployeeInsertDto employeeInsertDto)
        {
            ValidationResult result = await _employeeInsertValidator.ValidateAsync(employeeInsertDto);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            try
            {
                var employee = _mapper.Map<Employee>(employeeInsertDto);
                employee.CreatedAt = DateTime.Now;
                employee.UpdatedAt = DateTime.Now;

                await _employeeRepository.Add(employee);
                await _employeeRepository.Save();

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the employee.");
                throw new ServiceException("An error occurred while adding the employee.", ex);
            }
        }

        public async Task<EmployeeDto> Update(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            ValidationResult result = await _employeeUpdateValidator.ValidateAsync(employeeUpdateDto);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            try
            {
                var employee = await _employeeRepository.GetById(id);
                if (employee == null)
                {
                    return null;
                }

                employee = _mapper.Map(employeeUpdateDto, employee);
                employee.UpdatedAt = DateTime.Now;

                _employeeRepository.Update(employee);
                await _employeeRepository.Save();

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employee.");
                throw new ServiceException("An error occurred while updating the employee.", ex);
            }
        }

        public async Task<EmployeeDto> Delete(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetById(id);
                if (employee == null)
                {
                    return null;
                }

                _employeeRepository.Delete(employee);
                await _employeeRepository.Save();

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employee.");
                throw new ServiceException("An error occurred while deleting the employee.", ex);
            }
        }

        public async Task<IEnumerable<EmployeeDto>> Search(EmployeeSearchDto searchParams)
        {
            ValidationResult result = await _employeeSearchValidator.ValidateAsync(searchParams);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            try
            {
                var employees = await _employeeRepository.Search(searchParams);
                return employees.Select(e => _mapper.Map<EmployeeDto>(e));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employees.");
                throw new ServiceException("An error occurred while retrieving employees.", ex);
            }            
        }

    }
}
