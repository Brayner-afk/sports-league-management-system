using ClinicSystem.Domain.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class HistorialMedico : BaseEntity
    {
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Diagnostico { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente? Paciente { get; set; }

        public int MedicoId { get; set; }
        [ForeignKey("MedicoId")]
        public Medico? Medico { get; set; }

        public ICollection<Tratamiento> Tratamientos { get; set; } = new Collection<Tratamiento>();
    }
}
