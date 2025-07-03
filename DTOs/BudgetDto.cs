namespace PracticaAPI.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public DateTime Month { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal TotalSpent { get; set; }
    public int CategoriesCount { get; set; }
} 