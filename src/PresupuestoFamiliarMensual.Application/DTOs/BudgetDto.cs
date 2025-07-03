namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para representar un presupuesto
/// </summary>
public class BudgetDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public int FamilyMemberId { get; set; }
    public string FamilyMemberName { get; set; } = string.Empty;
    public int MonthId { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Propiedades calculadas
    public decimal TotalSpent { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsOverBudget { get; set; }
    public int CategoryCount { get; set; }
    public int ExpenseCount { get; set; }
} 