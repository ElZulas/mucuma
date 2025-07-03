namespace PracticaAPI.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public Guid MonthlyBudgetId { get; set; }
} 