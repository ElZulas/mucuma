using Microsoft.EntityFrameworkCore;
using PracticaAPI.Core.Entities;

namespace PracticaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
        });

        // Configuración para BudgetCategory
        modelBuilder.Entity<BudgetCategory>()
            .HasIndex(c => new { c.Name, c.MonthlyBudgetId })
            .IsUnique(); // prevenir nombres repetidos dentro de un mismo presupuesto

        // Configuración para MonthlyBudget
        modelBuilder.Entity<MonthlyBudget>()
            .HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}