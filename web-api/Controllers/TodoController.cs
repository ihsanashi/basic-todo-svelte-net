using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<GetTodoItemsResponse>> GetTodoItems()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new GetTodoItemsResponse
                {
                    Success = false,
                    ErrorMessage = "User is not authorized.",
                    Data = null
                });
            }

            var result = await _todoService.GetTodoItemsByUserAsync(userId);
            return Ok(result);
        }
        catch (ApplicationException exception)
        {
            Console.Error.WriteLine($"Error in GetTodoItems: {exception.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new GetTodoItemsResponse
            {
                Success = false,
                ErrorMessage = exception.Message,
                Data = null,
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new GetTodoItemsResponse
            {
                Success = false,
                ErrorMessage = ex.Message ?? "An unexpected error occured",
                Data = null,
            });
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authorized" });
            }

            var todoItem = await _todoService.GetTodoItemByIdAsync(id, userId);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authorized" });
            }

            var updatedItem = await _todoService.UpdateTodoItemAsync(id, todoDTO, userId);

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
        var userId = User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var createdItem = await _todoService.CreateTodoItemsAsync(todoDTO, userId);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authorized" });
            }

            var result = await _todoService.DeleteTodoItemsAsync(id, userId);
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
           Title = todoItem.Title,
           IsComplete = todoItem.IsComplete
       };
}