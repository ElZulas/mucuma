using Microsoft.EntityFrameworkCore;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Interfaces;
using PresupuestoFamiliarMensual.Infrastructure.Data;

namespace PresupuestoFamiliarMensual.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de presupuestos
/// </summary>
public class BudgetRepository : Repository<Budget>, IBudgetRepository
{
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Budget?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Budgets
            .Include(b => b.FamilyMember)
            .Include(b => b.Month)
            .Include(b => b.Categories)
            .Include(b => b.Expenses)
                .ThenInclude(e => e.BudgetCategory)
            .Include(b => b.Expenses)
                .ThenInclude(e => e.FamilyMember)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Budget>> GetByFamilyMemberAsync(int familyMemberId)
    {
        return await _context.Budgets
            .Include(b => b.FamilyMember)
            .Include(b => b.Month)
            .Include(b => b.Categories)
            .Include(b => b.Expenses)
            .Where(b => b.FamilyMemberId == familyMemberId)
            .ToListAsync();
    }

    public async Task<Budget?> GetByFamilyMemberAndMonthAsync(int familyMemberId, int monthId)
    {
        return await _context.Budgets
            .Include(b => b.FamilyMember)
            .Include(b => b.Month)
            .Include(b => b.Categories)
            .Include(b => b.Expenses)
            .FirstOrDefaultAsync(b => b.FamilyMemberId == familyMemberId && b.MonthId == monthId);
    }

    public async Task<IEnumerable<Budget>> GetAllWithDetailsAsync()
    {
        return await _context.Budgets
            .Include(b => b.FamilyMember)
            .Include(b => b.Month)
            .Include(b => b.Categories)
            .Include(b => b.Expenses)
                .ThenInclude(e => e.BudgetCategory)
            .Include(b => b.Expenses)
                .ThenInclude(e => e.FamilyMember)
            .ToListAsync();
    }
}
 