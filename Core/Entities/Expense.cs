namespace PracticaAPI.Core.Entities;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public Guid CategoryId { get; set; }
    public BudgetCategory? Category { get; set; }
}