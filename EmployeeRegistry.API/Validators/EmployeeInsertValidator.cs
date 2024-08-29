using FluentValidation;
using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Validators
{
    public class EmployeeInsertValidator : AbstractValidator<EmployeeInsertDto>
    {
        public EmployeeInsertValidator()
        {
            
        }
    }
}
