using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaAPI.Core.Entities;
using PracticaAPI.Data;
using PracticaAPI.DTOs;
using PracticaAPI.Core.Services.Interfaces;

namespace PracticaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _service;
    private readonly AppDbContext _context;

    public BudgetsController(IBudgetService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    // GET: api/budgets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets()
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        
        var budgets = await _context.MonthlyBudgets
            .Where(b => b.UserId == userId)
            .Include(b => b.Categories)
            .ThenInclude(c => c.Expenses)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Month = b.Month,
                TotalBudget = b.Categories.Sum(c => c.Limit),
                TotalSpent = b.Categories.Sum(c => c.Expenses.Sum(e => e.Amount)),
                CategoriesCount = b.Categories.Count
            })
            .ToListAsync();

        return Ok(budgets);
    }

    // GET: api/budgets/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDetailDto>> GetBudget(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        
        var budget = await _context.MonthlyBudgets
            .Where(b => b.Id == id && b.UserId == userId)
            .Include(b => b.Categories)
            .ThenInclude(c => c.Expenses)
            .FirstOrDefaultAsync();

        if (budget == null)
        {
            return NotFound("Presupuesto no encontrado");
        }

        var budgetDto = new BudgetDetailDto
        {
            Id = budget.Id,
            Month = budget.Month,
            Categories = budget.Categories.Select(c => new BudgetCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Limit = c.Limit,
                MonthlyBudgetId = c.MonthlyBudgetId,
                TotalSpent = c.Expenses.Sum(e => e.Amount)
            }).ToList()
        };

        return Ok(budgetDto);
    }

    // POST: api/budgets
    [HttpPost]
    public async Task<ActionResult<BudgetDto>> CreateBudget([FromBody] CreateBudgetDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var result = await _service.CreateBudgetAsync(dto, userId);
        
        var budgetDto = new BudgetDto
        {
            Id = result.Id,
            Month = result.Month,
            TotalBudget = result.Categories.Sum(c => c.Limit),
            TotalSpent = 0,
            CategoriesCount = result.Categories.Count
        };

        return CreatedAtAction(nameof(GetBudget), new { id = result.Id }, budgetDto);
    }

    // POST: api/budgets/{budgetId}/expenses
    [HttpPost("{budgetId}/expenses")]
    public async Task<ActionResult<ExpenseDto>> AddExpense(Guid budgetId, [FromBody] CreateExpenseDto dto)
    {
        try
        {
            var result = await _service.RegisterExpenseAsync(budgetId, dto);
            
            var expenseDto = new ExpenseDto
            {
                Id = result.Id,
                Amount = result.Amount,
                Date = result.Date,
                CategoryId = result.CategoryId,
                CategoryName = result.Category!.Name
            };

            return Ok(expenseDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/budgets/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        
        var budget = await _context.MonthlyBudgets
            .Where(b => b.Id == id && b.UserId == userId)
            .Include(b => b.Categories)
            .ThenInclude(c => c.Expenses)
            .FirstOrDefaultAsync();

        if (budget == null)
        {
            return NotFound("Presupuesto no encontrado");
        }

        // Verificar si tiene gastos
        var hasExpenses = budget.Categories.Any(c => c.Expenses.Any());
        if (hasExpenses)
        {
            return BadRequest("No se puede eliminar un presupuesto que tiene gastos registrados");
        }

        _context.MonthlyBudgets.Remove(budget);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/budgets/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] CreateBudgetDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        
        var budget = await _context.MonthlyBudgets
            .Where(b => b.Id == id && b.UserId == userId)
            .FirstOrDefaultAsync();

        if (budget == null)
        {
            return NotFound("Presupuesto no encontrado");
        }

        // Verificar si ya existe un presupuesto para el mismo mes
        var existingBudget = await _context.MonthlyBudgets
            .Where(b => b.Month == dto.Month && b.UserId == userId && b.Id != id)
            .FirstOrDefaultAsync();

        if (existingBudget != null)
        {
            return BadRequest("Ya existe un presupuesto para este mes");
        }

        budget.Month = dto.Month;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
