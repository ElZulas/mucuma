using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaAPI.Data;
using PracticaAPI.Core.Entities;
using PracticaAPI.DTOs;

namespace PracticaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Expenses?categoryId=xxx&page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetExpenses([FromQuery] Guid categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        var query = _context.Expenses.Where(e => e.CategoryId == categoryId);
        var total = await query.CountAsync();
        var expenses = await query
            .OrderByDescending(e => e.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(e => e.Category)
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                Date = e.Date,
                CategoryId = e.CategoryId,
                CategoryName = e.Category!.Name
            })
            .ToListAsync();
        return Ok(new { total, page, pageSize, expenses });
    }

    // GET: api/Expenses/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(Guid id)
    {
        var expense = await _context.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expense == null)
        {
            return NotFound("Gasto no encontrado");
        }

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category!.Name
        };

        return Ok(expenseDto);
    }

    // POST: api/Expenses
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense([FromBody] CreateExpenseDto dto)
    {
        // Verificar que la categoría existe
        var category = await _context.BudgetCategories
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

        if (category == null)
        {
            return BadRequest("La categoría especificada no existe");
        }

        // Verificar que no se exceda el límite de la categoría
        var totalSpent = category.Expenses.Sum(e => e.Amount);
        if (totalSpent + dto.Amount > category.Limit)
        {
            return BadRequest($"El gasto excede el límite de la categoría. Límite: {category.Limit}, Gastado: {totalSpent}, Nuevo gasto: {dto.Amount}");
        }

        var expense = new Expense
        {
            Amount = dto.Amount,
            Date = dto.Date,
            CategoryId = dto.CategoryId
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            CategoryName = category!.Name
        };

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expenseDto);
    }

    // PUT: api/Expenses/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto)
    {
        var expense = await _context.Expenses
            .Include(e => e.Category)
            .ThenInclude(c => c.Expenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expense == null)
        {
            return NotFound("Gasto no encontrado");
        }

        // Verificar que no se exceda el límite de la categoría
        var totalSpent = expense.Category.Expenses
            .Where(e => e.Id != id)
            .Sum(e => e.Amount);

        if (totalSpent + dto.Amount > expense.Category!.Limit)
        {
            return BadRequest($"El gasto excede el límite de la categoría. Límite: {expense.Category!.Limit}, Gastado: {totalSpent}, Nuevo gasto: {dto.Amount}");
        }

        expense.Amount = dto.Amount;
        expense.Date = dto.Date;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Expenses/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null)
        {
            return NotFound("Gasto no encontrado");
        }

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
