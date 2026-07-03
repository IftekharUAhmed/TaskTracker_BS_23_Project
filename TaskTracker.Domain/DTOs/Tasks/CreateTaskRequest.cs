using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedUserId { get; set; }
    }
}

