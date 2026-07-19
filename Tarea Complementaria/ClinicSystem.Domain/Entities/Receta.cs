using ClinicSystem.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Receta : BaseEntity
    {
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public string Dosis { get; set; } = string.Empty;
        public string Frecuencia { get; set; } = string.Empty;
        public int DuracionDias { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente? Paciente { get; set; }

        public int MedicamentoId { get; set; }
        [ForeignKey("MedicamentoId")]
        public Medicamento? Medicamento { get; set; }
    }
}
