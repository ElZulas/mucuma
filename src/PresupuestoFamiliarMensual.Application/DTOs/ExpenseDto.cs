namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para representar un gasto
/// </summary>
public class ExpenseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public int BudgetCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int BudgetId { get; set; }
    public int FamilyMemberId { get; set; }
    public string FamilyMemberName { get; set; } = string.Empty;
    public DateTime ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
} 