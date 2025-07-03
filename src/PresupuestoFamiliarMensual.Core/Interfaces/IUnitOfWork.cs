using PresupuestoFamiliarMensual.Core.Entities;

namespace PresupuestoFamiliarMensual.Core.Interfaces;

/// <summary>
/// Interfaz para la unidad de trabajo que maneja transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IBudgetRepository Budgets { get; }
    IBudgetCategoryRepository BudgetCategories { get; }
    IExpenseRepository Expenses { get; }
    IRepository<FamilyMember> FamilyMembers { get; }
    IRepository<Month> Months { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
} 