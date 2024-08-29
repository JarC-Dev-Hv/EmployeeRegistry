using FluentValidation;
using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Validators
{
    public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateValidator()
        {
            
        }
    }
}
