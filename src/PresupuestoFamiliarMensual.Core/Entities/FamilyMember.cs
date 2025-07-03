using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Core.Entities;

/// <summary>
/// Representa un miembro de la familia
/// </summary>
public class FamilyMember
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
} 