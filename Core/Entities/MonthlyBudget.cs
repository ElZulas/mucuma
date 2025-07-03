namespace PracticaAPI.Core.Entities;

public class MonthlyBudget
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Month { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<BudgetCategory> Categories { get; set; } = new();
}