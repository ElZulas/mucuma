using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Core.Entities;

/// <summary>
/// Representa un gasto registrado en una categoría específica
/// </summary>
public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Claves foráneas
    public int BudgetCategoryId { get; set; }
    public int FamilyMemberId { get; set; }
    public int MonthId { get; set; }
    
    // Relaciones de navegación
    public virtual BudgetCategory BudgetCategory { get; set; } = null!;
    public virtual FamilyMember FamilyMember { get; set; } = null!;
    public virtual Month Month { get; set; } = null!;
} 