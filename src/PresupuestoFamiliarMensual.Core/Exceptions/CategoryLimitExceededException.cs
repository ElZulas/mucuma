namespace PresupuestoFamiliarMensual.Core.Exceptions;

/// <summary>
/// Excepción que se lanza cuando se intenta registrar un gasto que excede el límite de la categoría
/// </summary>
public class CategoryLimitExceededException : DomainException
{
    public string CategoryName { get; }
    public decimal CurrentSpent { get; }
    public decimal Limit { get; }
    public decimal AttemptedAmount { get; }
    
    public CategoryLimitExceededException(string categoryName, decimal currentSpent, decimal limit, decimal attemptedAmount)
        : base($"No se puede registrar el gasto de ${attemptedAmount:F2} en la categoría '{categoryName}'. " +
               $"Ya se han gastado ${currentSpent:F2} de ${limit:F2} disponibles.")
    {
        CategoryName = categoryName;
        CurrentSpent = currentSpent;
        Limit = limit;
        AttemptedAmount = attemptedAmount;
    }
} 