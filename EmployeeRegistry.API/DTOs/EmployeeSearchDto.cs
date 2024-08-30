namespace EmployeeRegistry.API.DTOs
{
    public class EmployeeSearchDto : PaginationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
