using ClinicSystem.Domain.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClinicSystem.Domain.Entities
{
    public class Especialidad : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<Medico> Medicos { get; set; } = new Collection<Medico>();
    }
}
