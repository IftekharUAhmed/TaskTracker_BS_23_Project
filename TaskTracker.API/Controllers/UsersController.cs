using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Common;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var response = await _userManager.GetCurrentUserAsync(userId);
            return Ok(response);
        }

        //api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var response = await _userManager.GetPagedUsersAsync(page, pageSize);
            return Ok(response);
        }

        //api/users/{id}/role
        [HttpPatch("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(int id, [FromBody] UpdateUserRoleRequest request)
        {
            await _userManager.ChangeUserRoleAsync(id, request);

            return NoContent();
        }
    }
}