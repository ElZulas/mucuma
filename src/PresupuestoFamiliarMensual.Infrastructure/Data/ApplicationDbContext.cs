using Microsoft.EntityFrameworkCore;
using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Infrastructure.Data;

/// <summary>
/// Contexto de Entity Framework para la aplicación
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<FamilyMember> FamilyMembers { get; set; }
    public DbSet<Month> Months { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetCategory> BudgetCategories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de FamilyMember
        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configuración de Month
        modelBuilder.Entity<Month>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MonthNumber).IsRequired();
            entity.Property(e => e.Year).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasIndex(e => new { e.MonthNumber, e.Year }).IsUnique();
        });

        // Configuración de Budget
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.FamilyMemberId).IsRequired();
            entity.Property(e => e.MonthId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            
            entity.HasOne(e => e.FamilyMember)
                .WithMany()
                .HasForeignKey(e => e.FamilyMemberId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Month)
                .WithMany(m => m.Budgets)
                .HasForeignKey(e => e.MonthId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasIndex(e => new { e.FamilyMemberId, e.MonthId }).IsUnique();
        });

        // Configuración de BudgetCategory
        modelBuilder.Entity<BudgetCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Limit).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.BudgetId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
            
            entity.HasOne(e => e.Budget)
                .WithMany(b => b.Categories)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => new { e.BudgetId, e.Name }).IsUnique();
        });

        // Configuración de Expense
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.BudgetCategoryId).IsRequired();
            entity.Property(e => e.FamilyMemberId).IsRequired();
            entity.Property(e => e.MonthId).IsRequired();
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasOne(e => e.BudgetCategory)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.BudgetCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Month)
                .WithMany(m => m.Expenses)
                .HasForeignKey(e => e.MonthId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.FamilyMember)
                .WithMany(f => f.Expenses)
                .HasForeignKey(e => e.FamilyMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
} 