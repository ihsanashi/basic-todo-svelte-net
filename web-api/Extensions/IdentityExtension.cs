using Microsoft.AspNetCore.Identity;

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
  }
}