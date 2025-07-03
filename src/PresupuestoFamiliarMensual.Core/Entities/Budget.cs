using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Core.Entities;

/// <summary>
/// Representa el presupuesto mensual de la familia
/// </summary>
public class Budget
{
    public int Id { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }
    
    [Required]
    public int FamilyMemberId { get; set; }
    
    [Required]
    public int MonthId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public virtual FamilyMember FamilyMember { get; set; } = null!;
    public virtual Month Month { get; set; } = null!;
    public virtual ICollection<BudgetCategory> Categories { get; set; } = new List<BudgetCategory>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    
    // Propiedades calculadas
    public decimal TotalSpent => Expenses.Sum(e => e.Amount);
    public decimal RemainingAmount => TotalAmount - TotalSpent;
    public bool IsOverBudget => TotalSpent > TotalAmount;
} 