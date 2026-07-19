using ClinicSystem.Domain.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Factura : BaseEntity
    {
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagada, Anulada

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente? Paciente { get; set; }

        public ICollection<Pago> Pagos { get; set; } = new Collection<Pago>();
    }
}
