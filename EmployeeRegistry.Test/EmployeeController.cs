using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using EmployeeRegistry.API;
using EmployeeRegistry.API.DTOs;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;

namespace EmployeeRegistry.Test
{
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeeControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ShouldReturnAllEmployees()
        {
            // Act
            var response = await _client.GetAsync("/api/employees");

            // Assert
            response.EnsureSuccessStatusCode();
            var employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>();
            employees.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = 1; // Assuming an employee with ID 1 exists

            // Act
            var response = await _client.GetAsync($"/api/employees/{employeeId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            employee.Should().NotBeNull();
            employee!.Id.Should().Be(employeeId);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 9999; // Assuming an employee with this ID does not exist

            // Act
            var response = await _client.GetAsync($"/api/employees/{employeeId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostEmployee_ShouldReturnCreatedEmployee()
        {
            // Arrange
            var employeeDto = new EmployeeInsertDto
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-30),
                Salary = 1000
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/employees", employeeDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            createdEmployee.Should().NotBeNull();
            createdEmployee!.FirstName.Should().Be(employeeDto.FirstName);
        }

        [Fact]
        public async Task PutEmployee_ShouldReturnUpdatedEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = 1; // Assuming an employee with ID 1 exists
            var employeeUpdateDto = new EmployeeUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Salary = 1500
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/employees/{employeeId}", employeeUpdateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedEmployee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            updatedEmployee.Should().NotBeNull();
            updatedEmployee!.Salary.Should().Be(employeeUpdateDto.Salary);
        }

        [Fact]
        public async Task PutEmployee_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 9999; // Assuming an employee with this ID does not exist
            var employeeUpdateDto = new EmployeeUpdateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Salary = 1500
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/employees/{employeeId}", employeeUpdateDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnDeletedEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = 1; // Assuming an employee with ID 1 exists

            // Act
            var response = await _client.DeleteAsync($"/api/employees/{employeeId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var deletedEmployee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            deletedEmployee.Should().NotBeNull();
            deletedEmployee!.Id.Should().Be(employeeId);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 9999; // Assuming an employee with this ID does not exist

            // Act
            var response = await _client.DeleteAsync($"/api/employees/{employeeId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Search_ShouldReturnMatchingEmployees()
        {
            // Arrange
            var searchParams = new EmployeeSearchDto
            {
                FirstName = "John",
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var response = await _client.GetAsync($"/api/employees/search?FirstName={searchParams.FirstName}&PageNumber={searchParams.PageNumber}&PageSize={searchParams.PageSize}");

            // Assert
            response.EnsureSuccessStatusCode();
            var employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDto>>();
            employees.Should().NotBeNull();
            employees.Should().Contain(e => e.FirstName == "John");
        }
    }
}
