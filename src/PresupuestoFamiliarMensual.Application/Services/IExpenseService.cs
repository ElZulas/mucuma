using PresupuestoFamiliarMensual.Application.DTOs;

namespace PresupuestoFamiliarMensual.Application.Services;

/// <summary>
/// Interfaz para el servicio de gastos
/// </summary>
public interface IExpenseService
{
    Task<IEnumerable<ExpenseDto>> GetByBudgetIdAsync(int budgetId);
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<ExpenseDto> CreateAsync(int budgetId, CreateExpenseDto createExpenseDto);
    Task DeleteAsync(int id);
} 