using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }
        public int AuthorUserId { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        //navigation Properties
        public TaskItem TaskItem { get; set; } = null!;
        public User AuthorUser { get; set; } = null!;
    }
}