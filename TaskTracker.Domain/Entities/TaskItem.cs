using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities
{
    public class TaskItem
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
        public bool IsDeleted { get; set; }

        //navigation property
        public User? AssignedUser { get; set; }
        public User CreatedByUser { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; }

        public TaskItem()
        {
            Comments = new List<Comment>();
        }
    }
}