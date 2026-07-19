using ClinicSystem.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "Efectivo"; // Efectivo, Tarjeta, Transferencia

        public int FacturaId { get; set; }
        [ForeignKey("FacturaId")]
        public Factura? Factura { get; set; }
    }
}
