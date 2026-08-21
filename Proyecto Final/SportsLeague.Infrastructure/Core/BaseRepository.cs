using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Core;
using SportsLeague.Domain.Interfaces;
using SportsLeague.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SportsLeague.Infrastructure.Core
{
    /// <summary>
    /// Clase abstracta base que implementa las operaciones comunes de acceso a datos.
    /// Declara explícitamente la palabra clave abstract para cumplir con las directrices de POO.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que hereda de BaseEntity</typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        #region Sobrecarga de Constructores (Constructor Overloading)

        /// <summary>
        /// Constructor principal: Inicializa el repositorio con el contexto de la base de datos.
        /// </summary>
        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Sobrecarga de constructor: Permite inyectar un DbSet explícito o mockeado.
        /// </summary>
        protected BaseRepository(ApplicationDbContext context, DbSet<T> dbSet)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        #endregion

        #region Sobrecargas de Métodos (Method Overloading)

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Sobrecarga 1: Obtener entidades aplicando un filtro de predicado.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Sobrecarga 2: Obtener entidad por Id con control de AsNoTracking para optimización de lectura.
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id, bool asNoTracking)
        {
            if (asNoTracking)
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            }
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Sobrecarga 3: Agregar una colección de entidades en una sola transacción.
        /// </summary>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Sobrecarga 4: Verificar existencia bajo condición lambda.
        /// </summary>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        #endregion
    }
}

