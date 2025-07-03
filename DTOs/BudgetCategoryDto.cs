namespace PracticaAPI.DTOs;

public class BudgetCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public Guid MonthlyBudgetId { get; set; }
    public decimal TotalSpent { get; set; }
}