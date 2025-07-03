namespace PresupuestoFamiliarMensual.Core.Interfaces;

/// <summary>
/// Interfaz base para repositorios genéricos
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(int id);
} 