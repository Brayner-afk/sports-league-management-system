using ClinicSystem.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Cita : BaseEntity
    {
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Completada, Cancelada

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente? Paciente { get; set; }

        public int MedicoId { get; set; }
        [ForeignKey("MedicoId")]
        public Medico? Medico { get; set; }
    }
}
