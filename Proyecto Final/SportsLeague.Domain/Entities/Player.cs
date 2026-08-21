using SportsLeague.Domain.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLeague.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un Jugador perteneciente a un Equipo de la liga.
    /// Hereda de la clase abstracta BaseEntity.
    /// </summary>
    public class Player : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        // Llave foránea
        public int TeamId { get; set; }

        // Propiedad de navegación: Un jugador pertenece a un equipo
        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        #region Sobrecargas de Constructores (Constructor Overloading)

        /// <summary>
        /// Constructor por defecto (requerido por EF Core).
        /// </summary>
        public Player()
        {
        }

        /// <summary>
        /// Sobrecarga 1: Constructor con Nombre Completo, Posición y Id de Equipo.
        /// </summary>
        public Player(string fullName, string position, int teamId)
        {
            FullName = fullName;
            Position = position;
            TeamId = teamId;
        }

        /// <summary>
        /// Sobrecarga 2: Constructor completo con Id, Nombre Completo, Posición y Id de Equipo.
        /// </summary>
        public Player(int id, string fullName, string position, int teamId)
        {
            Id = id;
            FullName = fullName;
            Position = position;
            TeamId = teamId;
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        /// <summary>
        /// Sobrecarga 1: Actualizar nombre y posición sin cambiar equipo.
        /// </summary>
        public void UpdateInfo(string fullName, string position)
        {
            FullName = fullName;
            Position = position;
        }

        /// <summary>
        /// Sobrecarga 2: Actualizar nombre, posición y transferir de equipo.
        /// </summary>
        public void UpdateInfo(string fullName, string position, int newTeamId)
        {
            FullName = fullName;
            Position = position;
            TeamId = newTeamId;
        }

        #endregion

        #region Polimorfismo y Métodos Abstractos Implementados

        /// <summary>
        /// Implementación obligatoria del método abstracto de BaseEntity.
        /// </summary>
        public override string GetEntitySummary()
        {
            return $"Jugador: {FullName} | Posición: {Position} | Equipo ID: {TeamId}";
        }

        public override string ToString()
        {
            return $"{FullName} ({Position})";
        }

        #endregion
    }
}

