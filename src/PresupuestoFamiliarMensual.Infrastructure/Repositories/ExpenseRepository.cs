using Microsoft.EntityFrameworkCore;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Interfaces;
using PresupuestoFamiliarMensual.Infrastructure.Data;

namespace PresupuestoFamiliarMensual.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de gastos
/// </summary>
public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Expense?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Expenses
            .Include(e => e.BudgetCategory)
            .Include(e => e.Month)
            .Include(e => e.FamilyMember)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetByBudgetIdAsync(int budgetId)
    {
        return await _context.Expenses
            .Include(e => e.BudgetCategory)
            .Include(e => e.FamilyMember)
            .Where(e => e.MonthId == budgetId) // Cambiado de BudgetId a MonthId
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Expenses
            .Include(e => e.FamilyMember)
            .Where(e => e.BudgetCategoryId == categoryId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSpentByCategoryAsync(int categoryId)
    {
        return await _context.Expenses
            .Where(e => e.BudgetCategoryId == categoryId)
            .SumAsync(e => e.Amount);
    }

    public async Task<decimal> GetTotalSpentByBudgetAsync(int budgetId)
    {
        return await _context.Expenses
            .Where(e => e.MonthId == budgetId) // Cambiado de BudgetId a MonthId
            .SumAsync(e => e.Amount);
    }
} 