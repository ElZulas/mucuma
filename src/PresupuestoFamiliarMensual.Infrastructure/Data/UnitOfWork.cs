using Microsoft.EntityFrameworkCore.Storage;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Interfaces;
using PresupuestoFamiliarMensual.Infrastructure.Repositories;

namespace PresupuestoFamiliarMensual.Infrastructure.Data;

/// <summary>
/// Implementación de la unidad de trabajo
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Budgets = new BudgetRepository(context);
        BudgetCategories = new BudgetCategoryRepository(context);
        Expenses = new ExpenseRepository(context);
        FamilyMembers = new Repository<FamilyMember>(context);
        Months = new Repository<Month>(context);
    }

    public IBudgetRepository Budgets { get; }
    public IBudgetCategoryRepository BudgetCategories { get; }
    public IExpenseRepository Expenses { get; }
    public IRepository<FamilyMember> FamilyMembers { get; }
    public IRepository<Month> Months { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
} 