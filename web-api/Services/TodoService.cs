using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

  public async Task<IEnumerable<TodoItemDTO>> GetTodoItemsByUserAsync(string userId)
  {
    try
    {
      return await _context.TodoItems.Where(x => x.UserId == userId).Select(x => new TodoItemDTO
      {
        Id = x.Id,
        Name = x.Name,
        IsComplete = x.IsComplete,
      }).ToListAsync();
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error fetching todo items for userId {userId}: {exception.Message}");
      throw new ApplicationException("Unable to retrieve todo items for userId {userId}", exception);
    }
  }

  public async Task<TodoItemDTO?> GetTodoItemByIdAsync(long id, string userId)
  {
    try
    {
      var todoItem = await _context.TodoItems.Where(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();

      if (todoItem == null)
      {
        return null;
      }

      return ItemToDTO(todoItem);
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error fetching todo item with ID: {id}, {exception.Message}");
      throw new ApplicationException($"Unable to retrieve todo item with ID: {id}.", exception);
    }
  }

  public async Task<TodoItemDTO?> UpdateTodoItemAsync(long id, TodoItemDTO todoItemDTO, string userId)
  {
    if (id != todoItemDTO.Id)
    {
      return null;
    }

    var todoItem = await _context.TodoItems.Where(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();

    if (todoItem == null)
    {
      return null;
    }

    todoItem.Name = todoItemDTO.Name;
    todoItem.IsComplete = todoItemDTO.IsComplete;

    try
    {
      await _context.SaveChangesAsync();
      return new TodoItemDTO
      {
        Id = todoItemDTO.Id,
        Name = todoItemDTO.Name,
        IsComplete = todoItemDTO.IsComplete,
      };
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await TodoItemExistsAsync(id))
      {
        return null;
      }
      throw;
    }
  }

  public async Task<TodoItemDTO> CreateTodoItemsAsync(TodoItemDTO todoItemDTO, string userId)
  {
    var todoItem = new TodoItem
    {
      IsComplete = todoItemDTO.IsComplete,
      Name = todoItemDTO.Name,
      UserId = userId,
    };

    _context.TodoItems.Add(todoItem);
    await _context.SaveChangesAsync();

    return new TodoItemDTO
    {
      Id = todoItem.Id,
      Name = todoItem.Name,
      IsComplete = todoItem.IsComplete,
    };
  }

  public async Task<bool> DeleteTodoItemsAsync(long id, string userId)
  {
    var todoItem = await _context.TodoItems.Where(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();

    if (todoItem == null)
    {
      return false;
    }

    _context.TodoItems.Remove(todoItem);
    await _context.SaveChangesAsync();
    return true;
  }

  private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
     new TodoItemDTO
     {
       Id = todoItem.Id,
       Name = todoItem.Name,
       IsComplete = todoItem.IsComplete
     };

  private async Task<bool> TodoItemExistsAsync(long id)
  {
    return await _context.TodoItems.AnyAsync(e => e.Id == id);
  }
}