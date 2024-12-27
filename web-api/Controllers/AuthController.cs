using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Extensions;
using TodoApi.Models;

namespace TodoApi.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/auth")]
  [Tags("Authentication")]
  public class AuthController : ControllerBase
  {
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
      _signInManager = signInManager;
      _userManager = userManager;
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

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), 200)]  // Specifies the return type as UserDto
    [ProducesResponseType(typeof(object), 401)]  // Unauthorized response
    [ProducesResponseType(typeof(object), 500)]  // Internal Server Error response

    public async Task<IActionResult> GetCurrentUser()
    {
      var (success, user, errorMessage) = await IdentityExtension.GetCurrentUserAsync(HttpContext, _userManager);

      if (success)
      {
        return Ok(user);
      }

      return Unauthorized(new { Message = errorMessage });
    }
  }
}