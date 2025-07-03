namespace PresupuestoFamiliarMensual.Core.Exceptions;

/// <summary>
/// Excepción que se lanza cuando se intenta crear una categoría con un nombre que ya existe
/// </summary>
public class DuplicateCategoryNameException : DomainException
{
    public string CategoryName { get; }
    public int BudgetId { get; }
    
    public DuplicateCategoryNameException(string categoryName, int budgetId)
        : base($"Ya existe una categoría con el nombre '{categoryName}' en este presupuesto.")
    {
        CategoryName = categoryName;
        BudgetId = budgetId;
    }
} 