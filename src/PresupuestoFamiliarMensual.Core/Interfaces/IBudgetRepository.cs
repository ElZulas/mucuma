using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Core.Interfaces;

/// <summary>
/// Interfaz para el repositorio de presupuestos
/// </summary>
public interface IBudgetRepository : IRepository<Budget>
{
    Task<Budget?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Budget>> GetByFamilyMemberAsync(int familyMemberId);
    Task<Budget?> GetByFamilyMemberAndMonthAsync(int familyMemberId, int monthId);
    Task<IEnumerable<Budget>> GetAllWithDetailsAsync();
} 