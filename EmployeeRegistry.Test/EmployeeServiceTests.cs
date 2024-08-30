using Moq;
using Xunit;
using FluentAssertions;
using EmployeeRegistry.API.Services.EmployeeServices;
using EmployeeRegistry.API.Repository.EmployeeRepository;
using EmployeeRegistry.API.Models;
using EmployeeRegistry.API.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace EmployeeRegistry.Test
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<EmployeeInsertDto>> _employeeInsertValidatorMock;
        private readonly Mock<IValidator<EmployeeUpdateDto>> _employeeUpdateValidatorMock;
        private readonly Mock<IValidator<EmployeeSearchDto>> _employeeSearchValidatorMock;
        private readonly Mock<ILogger<EmployeeService>> _loggerMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _employeeInsertValidatorMock = new Mock<IValidator<EmployeeInsertDto>>();
            _employeeUpdateValidatorMock = new Mock<IValidator<EmployeeUpdateDto>>();
            _employeeSearchValidatorMock = new Mock<IValidator<EmployeeSearchDto>>();
            _loggerMock = new Mock<ILogger<EmployeeService>>();

            _employeeService = new EmployeeService(
                _employeeRepositoryMock.Object,
                _mapperMock.Object,
                _employeeInsertValidatorMock.Object,
                _employeeUpdateValidatorMock.Object,
                _employeeSearchValidatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { FirstName = "John", LastName = "Doe", Salary = 1000 },
                new Employee { FirstName = "Jane", LastName = "Smith", Salary = 2000 }
            };

            _employeeRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>())).Returns((Employee source) => new EmployeeDto { FirstName = source.FirstName, LastName = source.LastName });

            // Act
            var result = await _employeeService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.FirstName == "John" && e.LastName == "Doe");
            result.Should().Contain(e => e.FirstName == "Jane" && e.LastName == "Smith");
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee { FirstName = "John", LastName = "Doe", Salary = 1000 };

            _employeeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(new EmployeeDto { FirstName = "John", LastName = "Doe" });

            // Act
            var result = await _employeeService.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.GetById(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeInsertDto { FirstName = "John", LastName = "Doe", Salary = 1000 };
            var employee = new Employee { FirstName = "John", LastName = "Doe", Salary = 1000 };
            var createdEmployeeDto = new EmployeeDto { FirstName = "John", LastName = "Doe" };

            _employeeInsertValidatorMock.Setup(v => v.ValidateAsync(employeeDto, default)).ReturnsAsync(new ValidationResult());
            _mapperMock.Setup(m => m.Map<Employee>(employeeDto)).Returns(employee);
            _employeeRepositoryMock.Setup(repo => repo.Add(It.IsAny<Employee>())).Returns(Task.CompletedTask);
            _employeeRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(createdEmployeeDto);

            // Act
            var result = await _employeeService.Add(employeeDto);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            _employeeRepositoryMock.Verify(repo => repo.Add(It.IsAny<Employee>()), Times.Once);
        }


        [Fact]
        public async Task Update_ShouldReturnUpdatedEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeUpdateDto = new EmployeeUpdateDto { FirstName = "John", LastName = "Doe", Salary = 1500 };
            var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Salary = 1000 };
            var updatedEmployeeDto = new EmployeeDto { FirstName = "John", LastName = "Doe", Salary = 1500 };

            _employeeUpdateValidatorMock.Setup(v => v.ValidateAsync(employeeUpdateDto, default)).ReturnsAsync(new ValidationResult());
            _employeeRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map(employeeUpdateDto, employee)).Returns(employee);
            _employeeRepositoryMock.Setup(repo => repo.Update(employee)).Callback(() => { });
            _employeeRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(updatedEmployeeDto);

            // Act
            var result = await _employeeService.Update(1, employeeUpdateDto);

            // Assert
            result.Should().NotBeNull();
            result.Salary.Should().Be(1500);
            _employeeRepositoryMock.Verify(repo => repo.Update(employee), Times.Once);
        }




        [Fact]
        public async Task Update_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeUpdateDto = new EmployeeUpdateDto { FirstName = "John", LastName = "Doe", Salary = 1500 };

            _employeeUpdateValidatorMock.Setup(v => v.ValidateAsync(employeeUpdateDto, default)).ReturnsAsync(new ValidationResult());
            _employeeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.Update(1, employeeUpdateDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldReturnDeletedEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee { FirstName = "John", LastName = "Doe", Salary = 1000 };
            var employeeDto = new EmployeeDto { FirstName = "John", LastName = "Doe" };

            _employeeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(employeeDto);

            // Act
            var result = await _employeeService.Delete(1);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            _employeeRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Employee>()), Times.Once);
        }


        [Fact]
        public async Task Delete_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.Delete(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Search_ShouldReturnMatchingEmployees()
        {
            // Arrange
            var searchParams = new EmployeeSearchDto { FirstName = "John", PageNumber = 1, PageSize = 10 };
            var employees = new List<Employee>
            {
                new Employee { FirstName = "John", LastName = "Doe", Salary = 1000 },
                new Employee { FirstName = "John", LastName = "Smith", Salary = 2000 }
            };

            _employeeSearchValidatorMock.Setup(v => v.ValidateAsync(searchParams, default)).ReturnsAsync(new ValidationResult());
            _employeeRepositoryMock.Setup(repo => repo.Search(searchParams)).ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>())).Returns((Employee source) => new EmployeeDto { FirstName = source.FirstName, LastName = source.LastName });

            // Act
            var result = await _employeeService.Search(searchParams);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(e => e.FirstName == "John" && e.LastName == "Doe");
            result.Should().Contain(e => e.FirstName == "John" && e.LastName == "Smith");
        }
    }
}
