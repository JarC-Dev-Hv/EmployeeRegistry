using System;
using System.Collections.Generic;

namespace EmployeeRegistry.API.Models;

public partial class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string SecondLastName { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
