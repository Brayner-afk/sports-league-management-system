using System;

namespace SportsLeague.Domain.Core
{
    /// <summary>
    /// Clase base abstracta para todas las entidades del dominio de la Liga Deportiva.
    /// Define la identidad común (Id) y métodos polimórficos obligatorios.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// Método abstracto que cada entidad concreta debe implementar para describir su resumen.
        /// Demuestra el principio de abstracción y polimorfismo puro en POO.
        /// </summary>
        /// <returns>Descripción en texto de la entidad concreta.</returns>
        public abstract string GetEntitySummary();

        /// <summary>
        /// Método virtual con implementación base que puede ser sobreescrito si se requiere.
        /// </summary>
        public virtual string GetDisplayInfo() => $"[ID: {Id}] {GetEntitySummary()}";
    }
}


