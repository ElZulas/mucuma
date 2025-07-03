using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para crear un presupuesto
/// </summary>
public class CreateBudgetDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor a 0")]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un miembro de la familia")]
    public int FamilyMemberId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un mes")]
    public int MonthId { get; set; }
} 