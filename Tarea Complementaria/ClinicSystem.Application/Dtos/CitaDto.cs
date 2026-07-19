using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicSystem.Application.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la cita son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El motivo de la cita es obligatorio.")]
        [StringLength(200, ErrorMessage = "El motivo no puede superar los 200 caracteres.")]
        public string Motivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = "Pendiente";

        [Required(ErrorMessage = "El paciente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El paciente seleccionado no es válido.")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "El médico es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El médico seleccionado no es válido.")]
        public int MedicoId { get; set; }

        // Campos auxiliares para la interfaz de usuario
        public string? PacienteNombre { get; set; }
        public string? MedicoNombre { get; set; }
    }
}
