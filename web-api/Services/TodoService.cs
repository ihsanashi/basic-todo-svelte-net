using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService
{
  private readonly TodoContext _context;

  public TodoService(TodoContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<TodoItemDTO>> GetAllTodoItemsAsync()
  {
    try
    {
      return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error fetching todo items: {exception.Message}");
      throw new ApplicationException("Unable to retrieve todo items", exception);
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