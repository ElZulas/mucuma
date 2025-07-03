using Microsoft.AspNetCore.Mvc;
using PresupuestoFamiliarMensual.Application.DTOs;
using PresupuestoFamiliarMensual.Application.Services;
using PresupuestoFamiliarMensual.Core.Exceptions;

namespace PresupuestoFamiliarMensual.API.Controllers;

/// <summary>
/// Controlador para gestionar categorías de presupuesto
/// </summary>
[ApiController]
[Route("api/categories")]
public class BudgetCategoriesController : ControllerBase
{
    private readonly IBudgetCategoryService _categoryService;

    public BudgetCategoriesController(IBudgetCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Obtiene todas las categorías
    /// </summary>
    /// <returns>Lista de categorías</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetCategoryDto>>> GetAll()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todas las categorías de un presupuesto
    /// </summary>
    /// <param name="budgetId">ID del presupuesto</param>
    /// <returns>Lista de categorías</returns>
    [HttpGet("budget/{budgetId}")]
    public async Task<ActionResult<IEnumerable<BudgetCategoryDto>>> GetByBudgetId(int budgetId)
    {
        try
        {
            var categories = await _categoryService.GetByBudgetIdAsync(budgetId);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    /// <param name="id">ID de la categoría</param>
    /// <returns>Categoría encontrada</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetCategoryDto>> GetById(int id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"No se encontró la categoría con ID {id}" });

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea una nueva categoría
    /// </summary>
    /// <param name="createCategoryDto">Datos de la categoría a crear</param>
    /// <returns>Categoría creada</returns>
    [HttpPost]
    public async Task<ActionResult<BudgetCategoryDto>> Create(CreateBudgetCategoryDto createCategoryDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.CreateAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DuplicateCategoryNameException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una categoría existente
    /// </summary>
    /// <param name="id">ID de la categoría</param>
    /// <param name="updateCategoryDto">Datos actualizados de la categoría</param>
    /// <returns>Categoría actualizada</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetCategoryDto>> Update(int id, UpdateBudgetCategoryDto updateCategoryDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"No se encontró la categoría con ID {id}" });

            var updatedCategory = await _categoryService.UpdateAsync(id, updateCategoryDto);
            return Ok(updatedCategory);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DuplicateCategoryNameException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una categoría
    /// </summary>
    /// <param name="id">ID de la categoría</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"No se encontró la categoría con ID {id}" });

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (CategoryWithExpensesException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
        }
    }
} 