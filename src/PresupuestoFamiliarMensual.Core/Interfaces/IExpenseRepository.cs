using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Core.Interfaces;

/// <summary>
/// Interfaz para el repositorio de gastos
/// </summary>
public interface IExpenseRepository : IRepository<Expense>
{
    Task<Expense?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Expense>> GetByBudgetIdAsync(int budgetId);
    Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId);
    Task<decimal> GetTotalSpentByCategoryAsync(int categoryId);
    Task<decimal> GetTotalSpentByBudgetAsync(int budgetId);
} 