using SportsLeague.Domain.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SportsLeague.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un Equipo en la liga deportiva.
    /// Hereda de la clase abstracta BaseEntity.
    /// </summary>
    public class Team : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        // Propiedad de navegación: Un equipo tiene muchos jugadores
        public ICollection<Player> Players { get; set; } = new Collection<Player>();

        #region Sobrecargas de Constructores (Constructor Overloading)

        /// <summary>
        /// Constructor por defecto (requerido por EF Core).
        /// </summary>
        public Team()
        {
        }

        /// <summary>
        /// Sobrecarga 1: Constructor con Nombre y Ciudad.
        /// </summary>
        public Team(string name, string city)
        {
            Name = name;
            City = city;
        }

        /// <summary>
        /// Sobrecarga 2: Constructor con Id, Nombre y Ciudad.
        /// </summary>
        public Team(int id, string name, string city)
        {
            Id = id;
            Name = name;
            City = city;
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        /// <summary>
        /// Sobrecarga 1: Agregar jugador existente a la colección.
        /// </summary>
        public void AddPlayer(Player player)
        {
            player.TeamId = this.Id;
            player.Team = this;
            Players.Add(player);
        }

        /// <summary>
        /// Sobrecarga 2: Crear y agregar jugador mediante datos básicos.
        /// </summary>
        public void AddPlayer(string fullName, string position)
        {
            var player = new Player(fullName, position, this.Id);
            player.Team = this;
            Players.Add(player);
        }

        #endregion

        #region Polimorfismo y Métodos Abstractos Implementados

        /// <summary>
        /// Implementación obligatoria del método abstracto de BaseEntity.
        /// </summary>
        public override string GetEntitySummary()
        {
            return $"Equipo: {Name} | Ciudad: {City} | Cantidad de Jugadores: {Players?.Count ?? 0}";
        }

        public override string ToString()
        {
            return $"{Name} ({City})";
        }

        #endregion
    }
}

