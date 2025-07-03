using System.ComponentModel.DataAnnotations;

namespace PresupuestoFamiliarMensual.Core.Entities;

/// <summary>
/// Representa un mes del año
/// </summary>
public class Month
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int MonthNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    
    // Método para obtener el nombre del mes
    public static string GetMonthName(int monthNumber)
    {
        return monthNumber switch
        {
            1 => "Enero",
            2 => "Febrero",
            3 => "Marzo",
            4 => "Abril",
            5 => "Mayo",
            6 => "Junio",
            7 => "Julio",
            8 => "Agosto",
            9 => "Septiembre",
            10 => "Octubre",
            11 => "Noviembre",
            12 => "Diciembre",
            _ => throw new ArgumentException("Número de mes inválido", nameof(monthNumber))
        };
    }
} 