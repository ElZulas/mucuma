using Microsoft.AspNetCore.Mvc;
using PresupuestoFamiliarMensual.Application.DTOs;
using PresupuestoFamiliarMensual.Application.Services;
using PresupuestoFamiliarMensual.Core.Exceptions;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para gestionar gastos
/// </summary>
[ApiController]
[Route("api/budgets/{budgetId}/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Obtiene todos los gastos de un presupuesto
    /// </summary>
    /// <param name="budgetId">ID del presupuesto</param>
    /// <returns>Lista de gastos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByBudgetId(int budgetId)
    {
        try
        {
            var expenses = await _expenseService.GetByBudgetIdAsync(budgetId);
            return Ok(expenses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un gasto por su ID
    /// </summary>
    /// <param name="budgetId">ID del presupuesto</param>
    /// <param name="id">ID del gasto</param>
    /// <returns>Gasto encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetById(int budgetId, int id)
    {
        try
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
                return NotFound(new { message = $"No se encontró el gasto con ID {id}" });

            if (expense.BudgetId != budgetId)
                return BadRequest(new { message = "El gasto no pertenece al presupuesto especificado" });

            return Ok(expense);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo gasto en el presupuesto
    /// </summary>
    /// <param name="budgetId">ID del presupuesto</param>
    /// <param name="createExpenseDto">Datos del gasto a crear</param>
    /// <returns>Gasto creado</returns>
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(int budgetId, CreateExpenseDto createExpenseDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var expense = await _expenseService.CreateAsync(budgetId, createExpenseDto);
            return CreatedAtAction(nameof(GetById), new { budgetId, id = expense.Id }, expense);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (CategoryLimitExceededException ex)
        {
            return Conflict(new { 
                message = ex.Message,
                categoryName = ex.CategoryName,
                currentSpent = ex.CurrentSpent,
                limit = ex.Limit,
                attemptedAmount = ex.AttemptedAmount
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un gasto
    /// </summary>
    /// <param name="budgetId">ID del presupuesto</param>
    /// <param name="id">ID del gasto</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int budgetId, int id)
    {
        try
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null)
                return NotFound(new { message = $"No se encontró el gasto con ID {id}" });

            if (expense.BudgetId != budgetId)
                return BadRequest(new { message = "El gasto no pertenece al presupuesto especificado" });

            await _expenseService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
} 