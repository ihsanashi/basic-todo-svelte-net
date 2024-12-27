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

  public async Task<GetTodoItemsResponse> GetTodoItemsByUserAsync(string userId)
  {
    try
    {
      var items = await _context.TodoItems.Where(x => x.UserId == userId).Select(x => new TodoItemDTO
      {
        Id = x.Id,
        Title = x.Title,
        IsComplete = x.IsComplete,
        Description = x.Description,
        DueDate = x.DueDate,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
      }).ToListAsync();

      return new GetTodoItemsResponse
      {
        Success = true,
        Data = items
      };
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error fetching todo items for userId {userId}: {exception.Message}");
      return new GetTodoItemsResponse
      {
        Success = false,
        ErrorMessage = $"Unable to retrieve todo items for userId {userId}. {exception}",
      };
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

    todoItem.Title = todoItemDTO.Title;
    todoItem.IsComplete = todoItemDTO.IsComplete;
    todoItem.Description = todoItemDTO.Description;
    todoItem.DueDate = todoItemDTO.DueDate;
    todoItem.UpdatedAt = DateTime.UtcNow;

    try
    {
      await _context.SaveChangesAsync();
      return new TodoItemDTO
      {
        Id = todoItemDTO.Id,
        Title = todoItemDTO.Title,
        IsComplete = todoItemDTO.IsComplete,
        Description = todoItemDTO.Description,
        DueDate = todoItemDTO.DueDate,
        CreatedAt = todoItemDTO.CreatedAt,
        UpdatedAt = todoItemDTO.UpdatedAt
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
      Title = todoItemDTO.Title,
      IsComplete = todoItemDTO.IsComplete,
      Description = todoItemDTO.Description,
      DueDate = todoItemDTO.DueDate,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
      UserId = userId,
    };

    _context.TodoItems.Add(todoItem);
    await _context.SaveChangesAsync();

    return new TodoItemDTO
    {
      Id = todoItem.Id,
      Title = todoItem.Title,
      IsComplete = todoItem.IsComplete,
      Description = todoItem.Description,
      DueDate = todoItem.DueDate,
      CreatedAt = todoItem.CreatedAt,
      UpdatedAt = todoItem.UpdatedAt,
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
       Title = todoItem.Title,
       IsComplete = todoItem.IsComplete,
       Description = todoItem.Description,
       DueDate = todoItem.DueDate,
       CreatedAt = todoItem.CreatedAt,
       UpdatedAt = todoItem.UpdatedAt,
     };

  private async Task<bool> TodoItemExistsAsync(long id)
  {
    return await _context.TodoItems.AnyAsync(e => e.Id == id);
  }
}