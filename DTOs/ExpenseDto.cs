namespace PracticaAPI.DTOs;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
} 