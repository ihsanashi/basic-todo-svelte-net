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

  public async Task<TodoItemDTO?> GetTodoItemsByIdAsync(long id)
  {
    try
    {
      var todoItem = await _context.TodoItems.FindAsync(id);

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

  public async Task<TodoItemDTO?> UpdateTodoItemAsync(long id, TodoItemDTO todoItemDTO)
  {
    if (id != todoItemDTO.Id)
    {
      return null;
    }

    var todoItem = await _context.TodoItems.FindAsync(id);
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

  public async Task<TodoItemDTO> CreateTodoItemsAsync(TodoItemDTO todoItemDTO)
  {
    var todoItem = new TodoItem
    {
      IsComplete = todoItemDTO.IsComplete,
      Name = todoItemDTO.Name,
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

  public async Task<bool> DeleteTodoItemsAsync(long id)
  {
    var todoItem = await _context.TodoItems.FindAsync(id);
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