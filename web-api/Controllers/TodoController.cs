using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoItemsController(TodoService todoService)
    {
        _todoService = todoService;
    }

    // GET: api/TodoItems
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        try
        {
            var todoItems = await _todoService.GetAllTodoItemsAsync();
            return Ok(todoItems);
        }
        catch (ApplicationException exception)
        {
            Console.Error.WriteLine($"Error in GetTodoItems: {exception.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = exception.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
        }
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        try
        {
            var todoItem = await _todoService.GetTodoItemsByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound(new { message = $"Todo item with ID {id} not found." });
            }

            return Ok(todoItem);
        }
        catch (ApplicationException appException)
        {
            Console.Error.WriteLine($"Application error in GetTodoItem: {appException.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = appException.Message });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error in GetTodoItem: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
        }
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        try
        {
            var updatedItem = await _todoService.UpdateTodoItemAsync(id, todoDTO);
            if (updatedItem == null)
            {
                return NotFound(new { message = $"Todo item with ID {id} not found." });
            }

            return Ok(updatedItem);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error updating todo item with ID {id}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while updating the todo item with ID {id}" });
        }
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        try
        {
            var createdItem = await _todoService.CreateTodoItemsAsync(todoDTO);
            return CreatedAtAction(nameof(GetTodoItem), createdItem);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating todo item: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating a todo item." });
        }
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        try
        {
            var result = await _todoService.DeleteTodoItemsAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Todo item with ID {id} not found." });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting todo item with ID {id}.", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An error occurred while deleting the todo item with ID {id}" });
        }
    }

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
       new TodoItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };
}