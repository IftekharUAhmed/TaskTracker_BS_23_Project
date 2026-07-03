using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Common;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Domain.Interfaces
{
    public interface ITaskManager
    {
        Task<TaskWithCommentsResponse> GetTaskByIdAsync(int id);
        Task<TaskItemResponse> CreateTaskAsync(CreateTaskRequest request, int creatorUserId);
        Task UpdateTaskAsync(int id, UpdateTaskRequest request, int currentUserId, string currentUserRole);
        Task DeleteTaskAsync(int id, int currentUserId, string currentUserRole);

        Task AddCommentAsync(int taskId, CreateCommentRequest request, int authorUserId);
        Task<PagedResponse<TaskItemResponse>> GetTasksAsync(TaskFilterRequest filter, string currentUserRole);
    }
}