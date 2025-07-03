namespace PracticaAPI.DTOs;

public class CreateExpenseDto
{
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}