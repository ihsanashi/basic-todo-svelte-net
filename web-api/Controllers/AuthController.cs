using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Extensions;

namespace TodoApi.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/auth")]
  [Tags("Authentication")]
  public class AuthController : ControllerBase
  {
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(SignInManager<IdentityUser> signInManager)
    {
      _signInManager = signInManager;
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <param name="requestBody">A placeholder object for the request body.</param>
    /// <returns>NoContent if successful, Unauthorized or InternalServerError otherwise.</returns>
    ///
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] object? requestBody)
    {
      var (success, errorMessage) = await IdentityExtension.PerformLogoutAsync(_signInManager, requestBody);

      if (success)
      {
        return NoContent();
      }

      if (!string.IsNullOrEmpty(errorMessage) && errorMessage.StartsWith("Invalid"))
      {
        return Unauthorized(new { Message = errorMessage });
      }

      return StatusCode(500, new { Message = errorMessage });
    }
  }
}