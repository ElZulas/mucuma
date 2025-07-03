using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Core.Entities;

/// <summary>
/// Representa una categoría de gasto con un límite específico
/// </summary>
public class BudgetCategory
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Limit { get; set; }
    
    [Required]
    public int BudgetId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public virtual Budget Budget { get; set; } = null!;
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    
    // Propiedades calculadas
    public decimal TotalSpent => Expenses.Sum(e => e.Amount);
    public decimal RemainingAmount => Limit - TotalSpent;
    public bool IsOverLimit => TotalSpent > Limit;
    public bool HasExpenses => Expenses.Any();
} 