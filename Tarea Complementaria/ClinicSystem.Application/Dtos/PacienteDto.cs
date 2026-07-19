using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicSystem.Application.Dtos
{
    public class PacienteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre completo debe tener entre 3 y 150 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
        [StringLength(20, ErrorMessage = "El documento de identidad no puede superar los 20 caracteres.")]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres.")]
        public string Direccion { get; set; } = string.Empty;
    }
}
