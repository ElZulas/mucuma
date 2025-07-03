namespace PracticaAPI.Core.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // Relación con presupuestos
    public List<MonthlyBudget> Budgets { get; set; } = new();
} 