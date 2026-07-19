using ClinicSystem.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Tratamiento : BaseEntity
    {
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Indicaciones { get; set; } = string.Empty;

        public int HistorialMedicoId { get; set; }
        [ForeignKey("HistorialMedicoId")]
        public HistorialMedico? HistorialMedico { get; set; }
    }
}
