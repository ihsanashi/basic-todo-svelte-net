namespace TodoApi.Models;

public class TodoItem
{
  public long Id { get; set; }
  public required string Title { get; set; }
  public bool IsComplete { get; set; }
  public string? Description { get; set; }
  public DateTime? DueDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public required string UserId { get; set; }

  // Soft delete properties
  public bool? IsDeleted { get; set; }
  public DateTime? DeletedAt { get; set; }
}

public class TodoItemDTO
{
  public long Id { get; set; }
  public required string Title { get; set; }
  public bool IsComplete { get; set; }
  public string? Description { get; set; }
  public DateTime? DueDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  // Soft delete properties
  public bool? IsDeleted { get; set; }
  public DateTime? DeletedAt { get; set; }
}

public class GetTodoItemsResponse
{
  public bool Success { get; set; }
  public string? ErrorMessage { get; set; }
  public IEnumerable<TodoItemDTO>? Data { get; set; }
}

public class PostTodoItemsBulkSaveResponse
{
  public bool Success { get; set; }
  public string? ErrorMessage { get; set; }
  public IEnumerable<TodoItemDTO>? Data { get; set; }
}