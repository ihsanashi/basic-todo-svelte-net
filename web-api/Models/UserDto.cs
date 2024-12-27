using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models;

public class UserDto
{
  public required string Id { get; set; }
  public string? Email { get; set; }
  public required bool EmailConfirmed { get; set; }
  public string? PhoneNumber { get; set; }
  public required bool PhoneNumberConfirmed { get; set; }

  public static UserDto MapToUserDto(IdentityUser user)
  {
    return new UserDto
    {
      Id = user.Id,
      Email = user.Email,
      EmailConfirmed = user.EmailConfirmed,
      PhoneNumber = user.PhoneNumber,
      PhoneNumberConfirmed = user.PhoneNumberConfirmed,
    };
  }
}
