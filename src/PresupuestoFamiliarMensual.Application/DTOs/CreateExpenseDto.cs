using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para crear un gasto
/// </summary>
public class CreateExpenseDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Amount { get; set; }
    
    [Required]
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
    public int BudgetCategoryId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un miembro de la familia")]
    public int FamilyMemberId { get; set; }
    
    public DateTime? ExpenseDate { get; set; }
} 