using System.ComponentModel.DataAnnotations;

namespace ClinicSystem.Application.Dtos
{
    public class MedicoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo del médico es obligatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre del médico debe tener entre 3 y 150 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La licencia médica es obligatoria.")]
        [StringLength(50, ErrorMessage = "La licencia médica no puede superar los 50 caracteres.")]
        public string LicenciaMedica { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El identificador de la especialidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La especialidad debe ser válida.")]
        public int EspecialidadId { get; set; }
    }
}
