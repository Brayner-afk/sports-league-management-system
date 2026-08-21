using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SportsLeague.Domain.Core;

namespace SportsLeague.Domain.Interfaces
{
    /// <summary>
    /// Contrato base para repositorios genéricos con soporte a sobrecarga de métodos.
    /// </summary>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        // Sobrecarga 1: Obtener todos con filtrado mediante expresión lambda
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetByIdAsync(int id);

        // Sobrecarga 2: Obtener por Id con opción de no seguimiento (AsNoTracking)
        Task<T?> GetByIdAsync(int id, bool asNoTracking);

        Task AddAsync(T entity);

        // Sobrecarga 3: Agregar múltiples entidades por lote
        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        
        Task<bool> ExistsAsync(int id);

        // Sobrecarga 4: Verificar existencia según condición lambda
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}

