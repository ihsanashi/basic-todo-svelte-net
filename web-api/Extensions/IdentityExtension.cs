using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Extensions
{
  public static class IdentityExtension
  {
    public static IEndpointRouteBuilder MapLogoutEndpoint(this IEndpointRouteBuilder endpoints)
    {
      endpoints.MapPost("/logout", async (SignInManager<IdentityUser> signInManager, [FromBody] object empty) =>
      {
        if (empty != null)
        {
          await signInManager.SignOutAsync();
          return Results.NoContent();
        }
        return Results.Unauthorized();
      })
      .WithOpenApi()
      .RequireAuthorization();

      return endpoints;
    }
  }
}