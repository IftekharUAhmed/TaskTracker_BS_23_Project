using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        //api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authManager.RegisterAsync(request);
            return StatusCode(201);
        }

        //api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authManager.LoginAsync(request);
            return Ok(response);
        }

        //api/auth/refresh
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest request)
        {
            var response = await _authManager.RefreshTokenAsync(request);
            return Ok(response);
        }

        //api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RevokeTokenRequest request)
        {
            await _authManager.RevokeTokenAsync(request.RefreshToken);
            return NoContent();
        }
    }
}