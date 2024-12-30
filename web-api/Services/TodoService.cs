using Microsoft.AspNetCore.Http.HttpResults;
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

  public async Task<TodoItemsMultipleResponse> GetTodoItemsByUserAsync(string userId)
  {
    try
    {
      var items = await _context.TodoItems
      .Where(x => x.UserId == userId &&
      (x.IsDeleted == null || x.IsDeleted == false))
      .OrderByDescending(item => item.CreatedAt)
      .Select(x => new TodoItemDTO
      {
        Id = x.Id,
        Title = x.Title,
        IsComplete = x.IsComplete,
        Description = x.Description,
        DueDate = x.DueDate,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
      }).ToListAsync();

      return new TodoItemsMultipleResponse
      {
        Success = true,
        Data = items,
      };
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error fetching todo items for userId {userId}: {exception.Message}");
      return new TodoItemsMultipleResponse
      {
        Success = false,
        ErrorMessage = $"Unable to retrieve todo items for userId {userId}. {exception}",
      };
    }
  }

  public async Task<TodoItemResponse> GetTodoItemByIdAsync(long id, string userId)
  {
    try
    {
      var todoItem = await _context.TodoItems.Where(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();

      if (todoItem == null)
      {
        return new TodoItemResponse
        {
          Success = false,
          ErrorMessage = $"Todo item with ID {id} not found."
        };
      }

      var todoItemDto = new TodoItemDTO
      {
        Id = todoItem.Id,
        Title = todoItem.Title,
        IsComplete = todoItem.IsComplete,
        Description = todoItem.Description,
        DueDate = todoItem.DueDate,
        CreatedAt = todoItem.CreatedAt,
        UpdatedAt = todoItem.UpdatedAt,
      };

      return new TodoItemResponse
      {
        Success = true,
        Data = todoItemDto,
      };
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

  public async Task<TodoItemResponse> CreateTodoItemsAsync(TodoItemDTO todoItemDTO, string userId)
  {
    if (string.IsNullOrWhiteSpace(todoItemDTO.Title))
    {
      return new TodoItemResponse
      {
        Success = false,
        Data = null,
        ErrorMessage = "The title is required, and cannot be empty.",
      };
    }

    try
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

      var itemRes = new TodoItemDTO
      {
        Id = todoItem.Id,
        Title = todoItem.Title,
        IsComplete = todoItem.IsComplete,
        Description = todoItem.Description,
        DueDate = todoItem.DueDate,
        CreatedAt = todoItem.CreatedAt,
        UpdatedAt = todoItem.UpdatedAt,
      };

      return new TodoItemResponse
      {
        Success = true,
        Data = itemRes,
        ErrorMessage = null,
      };
    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error creating a todo item for userId {userId}: {exception.Message}");

      return new TodoItemResponse
      {
        Data = null,
        Success = false,
        ErrorMessage = $"Unable to save a todo item for userId {userId}"
      };
    }
  }

  public async Task<TodoItemsMultipleResponse> SaveTodoItemsAsync(IEnumerable<TodoItemDTO> todoItems, string userId)
  {
    try
    {
      var savedItems = new List<TodoItemDTO>();

      foreach (var todoItem in todoItems)
      {
        // Check if the item exists (update) or is new (create)
        var existingItem = todoItem.Id != 0
        ? await _context.TodoItems.FirstOrDefaultAsync(item => item.Id == todoItem.Id && item.UserId == userId)
        : null;

        if (existingItem != null)
        {
          // Update existing item
          existingItem.Title = todoItem.Title;
          existingItem.IsComplete = todoItem.IsComplete;
          existingItem.Description = todoItem.Description;
          existingItem.DueDate = todoItem.DueDate;
          existingItem.UpdatedAt = DateTime.UtcNow;

          savedItems.Add(new TodoItemDTO
          {
            Id = existingItem.Id,
            Title = existingItem.Title,
            IsComplete = existingItem.IsComplete,
            Description = existingItem.Description,
            DueDate = existingItem.DueDate,
            CreatedAt = existingItem.CreatedAt,
            UpdatedAt = existingItem.UpdatedAt,
          });
        }
        else
        {
          // Create new item
          var newItem = new TodoItem
          {
            Title = todoItem.Title,
            IsComplete = todoItem.IsComplete,
            Description = todoItem.Description,
            DueDate = todoItem.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
            IsDeleted = false,
          };

          _context.TodoItems.Add(newItem);

          savedItems.Add(new TodoItemDTO
          {
            Id = newItem.Id,
            Title = newItem.Title,
            IsComplete = newItem.IsComplete,
            Description = newItem.Description,
            DueDate = newItem.DueDate,
            CreatedAt = newItem.CreatedAt,
            UpdatedAt = newItem.UpdatedAt,
            IsDeleted = false,
          });
        }
      }

      await _context.SaveChangesAsync();

      return new TodoItemsMultipleResponse
      {
        Success = true,
        Data = savedItems,
        ErrorMessage = null,
      };

    }
    catch (Exception exception)
    {
      Console.Error.WriteLine($"Error saving todo items in bulk for userId {userId}: {exception.Message}");
      return new TodoItemsMultipleResponse
      {
        Data = null,
        Success = false,
        ErrorMessage = $"Unable to save todo items in bulk for userId {userId}"
      };
    }
  }

  public async Task<TodoItemDeletionResponse> DeleteTodoItemsAsync(long id, string userId, bool permanentDelete = false)
  {
    var todoItem = await _context.TodoItems
      .Where(t => t.Id == id && t.UserId == userId
      && (t.IsDeleted == null || t.IsDeleted == false))
      .FirstOrDefaultAsync();

    if (todoItem == null)
    {
      return new TodoItemDeletionResponse
      {
        Success = false,
        ErrorMessage = $"Todo item with ID {id} not found, or already deleted."
      };
    }

    if (permanentDelete)
    {
      _context.TodoItems.Remove(todoItem);
    }
    else
    {
      todoItem.IsDeleted = true;
      todoItem.DeletedAt = DateTime.UtcNow;
      _context.TodoItems.Update(todoItem);
    }

    await _context.SaveChangesAsync();

    return new TodoItemDeletionResponse
    {
      Success = true,
      ErrorMessage = null,
    };
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