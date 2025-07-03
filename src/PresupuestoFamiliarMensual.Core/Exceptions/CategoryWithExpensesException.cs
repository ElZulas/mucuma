namespace PresupuestoFamiliarMensual.Core.Exceptions;

/// <summary>
/// Excepción que se lanza cuando se intenta eliminar una categoría que tiene gastos registrados
/// </summary>
public class CategoryWithExpensesException : DomainException
{
    public string CategoryName { get; }
    public int CategoryId { get; }
    public int ExpenseCount { get; }
    
    public CategoryWithExpensesException(string categoryName, int categoryId, int expenseCount)
        : base($"No se puede eliminar la categoría '{categoryName}' porque tiene {expenseCount} gasto(s) registrado(s). " +
               "Para mantener la consistencia de los datos, primero debe eliminar todos los gastos de esta categoría.")
    {
        CategoryName = categoryName;
        CategoryId = categoryId;
        ExpenseCount = expenseCount;
    }
} 