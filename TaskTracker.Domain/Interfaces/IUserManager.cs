using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Common;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Domain.Interfaces
{
    public interface IUserManager
    {
        Task<UserResponse> GetCurrentUserAsync(int userId);
        Task<PagedResponse<UserResponse>> GetPagedUsersAsync(int page, int pageSize);
        Task ChangeUserRoleAsync(int targetUserId, UpdateUserRoleRequest request);
    }
}