using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TodoApi.Models;

namespace TodoApi.Extensions
{
  public static class IdentityExtension
  {
    public static async Task<(bool success, string? ErrorMessage)> PerformLogoutAsync(SignInManager<IdentityUser> signInManager, object? requestBody)
    {
      if (requestBody == null)
      {
        return (false, "Invalid request body.");
      }

      try
      {
        await signInManager.SignOutAsync();
        return (true, null);
      }
      catch (Exception ex)
      {
        return (false, $"An error occurred during logout: {ex.Message}");
      }
    }

    public static async Task<(bool success, UserDto? user, string? errorMessage)> GetCurrentUserAsync(HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
      var userClaims = httpContext.User;
      if (userClaims.Identity?.IsAuthenticated == true)
      {
        var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
          return (false, null, "User ID not found in the claims.");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
          var userDto = UserDto.MapToUserDto(user);
          return (true, userDto, null);
        }
        else
        {
          return (false, null, "User not found.");
        }
      }

      return (false, null, "User is not authenticated.");
    }
  }
}