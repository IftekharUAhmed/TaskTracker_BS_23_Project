using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.DTOs.Users
{
    public class ChangeRoleRequest
    {
        [Required]
        public string Role { get; set; } = null!;
    }
}
