using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models;

public class TodoItem
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public bool IsComplete { get; set; }
  public string? Secret { get; set; }
  public required string UserId { get; set; }
}

public class TodoItemDTO
{
  public long Id { get; set; }
  public string? Name { get; set; }
  public bool IsComplete { get; set; }
}