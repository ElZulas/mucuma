using Microsoft.EntityFrameworkCore;
using PracticaAPI.Core.Entities;
using PracticaAPI.Core.Services.Interfaces;
using PracticaAPI.Data;
using PracticaAPI.DTOs;

namespace PracticaAPI.Core.Services;

public class BudgetService : IBudgetService
{
    private readonly AppDbContext _context;

    public BudgetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MonthlyBudget> CreateBudgetAsync(CreateBudgetDto dto, Guid userId)
    {
        var budget = new MonthlyBudget
        {
            Month = dto.Month,
            UserId = userId,
            Categories = dto.Categories
                .GroupBy(c => c.Name.ToLower()) // Evitar repetidos
                .Select(g => new BudgetCategory { Name = g.Key, Limit = g.First().Limit })
                .ToList()
        };

        _context.MonthlyBudgets.Add(budget);
        await _context.SaveChangesAsync();
        return budget;
    }

    public async Task<Expense> RegisterExpenseAsync(Guid budgetId, CreateExpenseDto dto)
    {
        var budget = await _context.MonthlyBudgets
            .Include(b => b.Categories)
            .ThenInclude(c => c.Expenses)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null) throw new Exception("Presupuesto no encontrado");

        var category = budget.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
        if (category == null) throw new Exception("Categoría no encontrada");

        if (category.TotalSpent + dto.Amount > category.Limit)
            throw new Exception("Gasto excede el límite permitido");

        var expense = new Expense
        {
            Amount = dto.Amount,
            CategoryId = dto.CategoryId
        };

        category.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return expense;
    }
}
