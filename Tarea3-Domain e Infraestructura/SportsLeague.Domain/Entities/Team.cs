using SportsLeague.Domain.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SportsLeague.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        // Propiedad de navegación: Un equipo tiene muchos jugadores
        public ICollection<Player> Players { get; set; } = new Collection<Player>();
    }
}
