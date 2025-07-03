namespace PracticaAPI.DTOs;

public class BudgetDetailDto
{
    public Guid Id { get; set; }
    public DateTime Month { get; set; }
    public List<BudgetCategoryDto> Categories { get; set; } = new();
} 