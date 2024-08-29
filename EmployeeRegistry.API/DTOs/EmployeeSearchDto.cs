namespace EmployeeRegistry.API.DTOs
{
    public class EmployeeSearchDto : PaginationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}
