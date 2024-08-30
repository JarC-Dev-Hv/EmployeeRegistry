using FluentValidation;
using EmployeeRegistry.API.DTOs;

namespace EmployeeRegistry.API.Validators
{
    public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El primer nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El primer nombre no puede tener más de 50 caracteres.")
                .Matches(@"^[a-zA-Z]+$").WithMessage("El primer nombre solo puede contener letras.");

            RuleFor(x => x.MiddleName)
                .MaximumLength(50).WithMessage("El segundo nombre no puede tener más de 50 caracteres.")
                .Matches(@"^[a-zA-Z]*$").WithMessage("El segundo nombre solo puede contener letras.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El primer apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El primer apellido no puede tener más de 50 caracteres.")
                .Matches(@"^[a-zA-Z]+$").WithMessage("El primer apellido solo puede contener letras.");

            RuleFor(x => x.SecondLastName)
                .MaximumLength(50).WithMessage("El segundo apellido no puede tener más de 50 caracteres.")
                .Matches(@"^[a-zA-Z]*$").WithMessage("El segundo apellido solo puede contener letras.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.");

            RuleFor(x => x.Salary)
                .NotEmpty().WithMessage("El sueldo es obligatorio.")
                .GreaterThan(0).WithMessage("El sueldo debe ser mayor que 0.");
        }
    }
}
