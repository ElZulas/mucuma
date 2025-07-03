namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para representar una categoría de presupuesto
/// </summary>
public class BudgetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public int BudgetId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Propiedades calculadas
    public decimal TotalSpent { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsOverLimit { get; set; }
    public int ExpenseCount { get; set; }
} 