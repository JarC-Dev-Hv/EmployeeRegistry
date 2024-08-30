using FluentValidation;
using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Validators
{
    public class EmployeeSearchValidator : AbstractValidator<EmployeeSearchDto>
    {
        public EmployeeSearchValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("El número de página debe ser mayor que 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor que 0.");
        }
    }
}
