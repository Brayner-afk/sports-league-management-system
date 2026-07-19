using SportsLeague.Domain.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLeague.Domain.Entities
{
    public class Player : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        // Llave foránea
        public int TeamId { get; set; }

        // Propiedad de navegación: Un jugador pertenece a un equipo
        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
    }
}
