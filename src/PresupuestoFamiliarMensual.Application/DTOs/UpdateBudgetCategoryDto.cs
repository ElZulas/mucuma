using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Application.DTOs;

/// <summary>
/// DTO para actualizar una categoría de presupuesto
/// </summary>
public class UpdateBudgetCategoryDto
{
    [Required]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El límite debe ser mayor a 0")]
    public decimal Limit { get; set; }
} 