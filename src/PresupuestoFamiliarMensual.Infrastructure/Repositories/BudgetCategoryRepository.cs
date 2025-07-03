using Microsoft.EntityFrameworkCore;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Interfaces;
using PresupuestoFamiliarMensual.Infrastructure.Data;

namespace PresupuestoFamiliarMensual.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de categorías de presupuesto
/// </summary>
public class BudgetCategoryRepository : Repository<BudgetCategory>, IBudgetCategoryRepository
{
    public BudgetCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<BudgetCategory?> GetByIdWithExpensesAsync(int id)
    {
        return await _context.BudgetCategories
            .Include(c => c.Budget)
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<BudgetCategory>> GetByBudgetIdAsync(int budgetId)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .Where(c => c.BudgetId == budgetId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNameInBudgetAsync(string name, int budgetId, int? excludeId = null)
    {
        var query = _context.BudgetCategories
            .Where(c => c.BudgetId == budgetId && c.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<BudgetCategory?> GetByNameInBudgetAsync(string name, int budgetId)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.BudgetId == budgetId && c.Name.ToLower() == name.ToLower());
    }
} 