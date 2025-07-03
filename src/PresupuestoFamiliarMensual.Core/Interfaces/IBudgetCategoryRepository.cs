using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Core.Interfaces;

/// <summary>
/// Interfaz para el repositorio de categorías de presupuesto
/// </summary>
public interface IBudgetCategoryRepository : IRepository<BudgetCategory>
{
    Task<BudgetCategory?> GetByIdWithExpensesAsync(int id);
    Task<IEnumerable<BudgetCategory>> GetByBudgetIdAsync(int budgetId);
    Task<bool> ExistsByNameInBudgetAsync(string name, int budgetId, int? excludeId = null);
    Task<BudgetCategory?> GetByNameInBudgetAsync(string name, int budgetId);
} 