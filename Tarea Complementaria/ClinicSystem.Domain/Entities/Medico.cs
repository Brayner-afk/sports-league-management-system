using ClinicSystem.Domain.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicSystem.Domain.Entities
{
    public class Medico : BaseEntity
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string LicenciaMedica { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public int EspecialidadId { get; set; }
        [ForeignKey("EspecialidadId")]
        public Especialidad? Especialidad { get; set; }

        public ICollection<Cita> Citas { get; set; } = new Collection<Cita>();
    }
}
