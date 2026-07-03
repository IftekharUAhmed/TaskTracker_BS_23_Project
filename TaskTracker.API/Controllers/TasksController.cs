using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TasksController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TasksController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        //api/tasks
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterRequest filter)
        {
            var response = await _taskManager.GetTasksAsync(filter, GetCurrentUserRole());
            return Ok(response);
        }

        //api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var response = await _taskManager.GetTaskByIdAsync(id);
            return Ok(response);
        }

        //api/tasks
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            int currentUserId = GetCurrentUserId();
            var response = await _taskManager.CreateTaskAsync(request, currentUserId);

            // Return 201 Created
            return CreatedAtAction(nameof(GetTask), new { id = response.Id }, response);
        }

        //api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            await _taskManager.UpdateTaskAsync(id, request, GetCurrentUserId(), GetCurrentUserRole());
            return NoContent();
        }

        // api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskManager.DeleteTaskAsync(id, GetCurrentUserId(), GetCurrentUserRole());
            return NoContent();
        }

        //api/tasks/{id}/comments
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CreateCommentRequest request)
        {
            await _taskManager.AddCommentAsync(id, request, GetCurrentUserId());
            return StatusCode(201); 
        }
    }
}
