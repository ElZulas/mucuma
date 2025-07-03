using AutoMapper;
using PresupuestoFamiliarMensual.Application.DTOs;
using PresupuestoFamiliarMensual.Core.Entities;
using PresupuestoFamiliarMensual.Core.Exceptions;
using PresupuestoFamiliarMensual.Core.Interfaces;

namespace PresupuestoFamiliarMensual.Application.Services;

/// <summary>
/// Implementación del servicio de categorías de presupuesto
/// </summary>
public class BudgetCategoryService : IBudgetCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BudgetCategoryDto>> GetAllAsync()
    {
        var categories = await _unitOfWork.BudgetCategories.GetAllAsync();
        return _mapper.Map<IEnumerable<BudgetCategoryDto>>(categories);
    }

    public async Task<IEnumerable<BudgetCategoryDto>> GetByBudgetIdAsync(int budgetId)
    {
        var categories = await _unitOfWork.BudgetCategories.GetByBudgetIdAsync(budgetId);
        return _mapper.Map<IEnumerable<BudgetCategoryDto>>(categories);
    }

    public async Task<BudgetCategoryDto?> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.BudgetCategories.GetByIdWithExpensesAsync(id);
        return _mapper.Map<BudgetCategoryDto>(category);
    }

    public async Task<BudgetCategoryDto> CreateAsync(CreateBudgetCategoryDto createCategoryDto)
    {
        // Verificar que el presupuesto existe
        var budget = await _unitOfWork.Budgets.GetByIdAsync(createCategoryDto.BudgetId);
        if (budget == null)
            throw new ArgumentException($"No se encontró el presupuesto con ID {createCategoryDto.BudgetId}");

        // REGLA DE NEGOCIO: No permitir categorías con nombre repetido
        var normalizedName = createCategoryDto.Name.Trim();
        var existsByName = await _unitOfWork.BudgetCategories.ExistsByNameInBudgetAsync(normalizedName, createCategoryDto.BudgetId);
        if (existsByName)
            throw new DuplicateCategoryNameException(normalizedName, createCategoryDto.BudgetId);

        var category = new BudgetCategory
        {
            Name = normalizedName,
            Limit = createCategoryDto.Limit,
            BudgetId = createCategoryDto.BudgetId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BudgetCategories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var createdCategory = await _unitOfWork.BudgetCategories.GetByIdWithExpensesAsync(category.Id);
        return _mapper.Map<BudgetCategoryDto>(createdCategory);
    }

    public async Task<BudgetCategoryDto> UpdateAsync(int id, UpdateBudgetCategoryDto updateCategoryDto)
    {
        var category = await _unitOfWork.BudgetCategories.GetByIdWithExpensesAsync(id);
        if (category == null)
            throw new ArgumentException($"No se encontró la categoría con ID {id}");

        // REGLA DE NEGOCIO: No permitir categorías con nombre repetido (excluyendo la actual)
        var normalizedName = updateCategoryDto.Name.Trim();
        var existsByName = await _unitOfWork.BudgetCategories.ExistsByNameInBudgetAsync(normalizedName, category.BudgetId, id);
        if (existsByName)
            throw new DuplicateCategoryNameException(normalizedName, category.BudgetId);

        category.Name = normalizedName;
        category.Limit = updateCategoryDto.Limit;
        category.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.BudgetCategories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var updatedCategory = await _unitOfWork.BudgetCategories.GetByIdWithExpensesAsync(category.Id);
        return _mapper.Map<BudgetCategoryDto>(updatedCategory);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.BudgetCategories.GetByIdWithExpensesAsync(id);
        if (category == null)
            throw new ArgumentException($"No se encontró la categoría con ID {id}");

        // REGLA DE NEGOCIO: No permitir eliminar categorías con gastos registrados
        if (category.HasExpenses)
            throw new CategoryWithExpensesException(category.Name, category.Id, category.Expenses.Count);

        await _unitOfWork.BudgetCategories.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
} 