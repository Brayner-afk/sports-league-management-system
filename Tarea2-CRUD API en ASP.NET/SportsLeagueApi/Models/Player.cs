using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLeagueApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        // Llave foránea
        public int TeamId { get; set; }

        // Propiedad de navegación: Un jugador pertenece a un equipo
        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
    }
}
