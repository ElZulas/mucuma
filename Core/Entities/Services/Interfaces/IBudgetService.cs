using PracticaAPI.DTOs;
using PracticaAPI.Core.Entities;

namespace PracticaAPI.Core.Services.Interfaces;

public interface IBudgetService
{
    Task<MonthlyBudget> CreateBudgetAsync(CreateBudgetDto dto, Guid userId);
    Task<Expense> RegisterExpenseAsync(Guid budgetId, CreateExpenseDto dto);
    // Más métodos para las categorías (CRUD)
}
