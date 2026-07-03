using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks
{
    public class TaskFilterRequest
    {
        public Enums.TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public int? AssigneeId { get; set; }
        public DateTime? DueBefore { get; set; }
        public DateTime? DueAfter { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool IncludeDeleted { get; set; } = false;
    }
}
