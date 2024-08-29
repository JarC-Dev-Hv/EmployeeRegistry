using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Services.EmployeeServices;
using EmployeeRegistry.API.Utilities;

namespace EmployeeRegistry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Search([FromQuery] EmployeeSearchDto searchParams)
        {
            try
            {
                var employees = await _employeeService.Search(searchParams);
                return Ok(employees);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while retrieving employees.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for employees.");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
        {
            try
            {
                var employee = await _employeeService.GetAll();
                return Ok(employee);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while retrieving employees.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving employees.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            try
            {
                var employee = await _employeeService.GetById(id);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while retrieving the employee.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving the employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Add(EmployeeInsertDto employeeInsertDto)
        {
            try
            {
                var employee = await _employeeService.Add(employeeInsertDto);
                return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred while adding the employee.");
                return BadRequest(ex.Errors);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while adding the employee.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> Update(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            try
            {
                var employee = await _employeeService.Update(id, employeeUpdateDto);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred while updating the employee.");
                return BadRequest(ex.Errors);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while updating the employee.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDto>> Delete(int id)
        {
            try
            {
                var employee = await _employeeService.Delete(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while deleting the employee.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the employee.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
