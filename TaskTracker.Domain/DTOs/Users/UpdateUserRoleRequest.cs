using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.DTOs.Users
{
    public class UpdateUserRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }
}