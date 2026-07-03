using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks
{
    public class TaskItemResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}