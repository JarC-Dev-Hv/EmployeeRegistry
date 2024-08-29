using System.ComponentModel.DataAnnotations;

namespace EmployeeRegistry.API.DTOs
{
    public partial class EmployeeInsertDto
    {
        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El primer nombre no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer nombre solo puede contener letras.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "El segundo nombre no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "El segundo nombre solo puede contener letras.")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El primer apellido no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El primer apellido solo puede contener letras.")]
        public string LastName { get; set; }

        [MaxLength(50, ErrorMessage = "El segundo apellido no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "El segundo apellido solo puede contener letras.")]
        public string SecondLastName { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El sueldo es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El sueldo debe ser mayor que 0.")]
        public decimal Salary { get; set; }
    }
}
