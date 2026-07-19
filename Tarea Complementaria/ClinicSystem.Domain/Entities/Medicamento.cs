using ClinicSystem.Domain.Core;

namespace ClinicSystem.Domain.Entities
{
    public class Medicamento : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Concentracion { get; set; } = string.Empty;
        public string Presentacion { get; set; } = string.Empty;
    }
}
