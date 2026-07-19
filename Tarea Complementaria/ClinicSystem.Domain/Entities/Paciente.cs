using ClinicSystem.Domain.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClinicSystem.Domain.Entities
{
    public class Paciente : BaseEntity
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        public ICollection<Cita> Citas { get; set; } = new Collection<Cita>();
        public ICollection<HistorialMedico> Historiales { get; set; } = new Collection<HistorialMedico>();
        public ICollection<Factura> Facturas { get; set; } = new Collection<Factura>();
    }
}
