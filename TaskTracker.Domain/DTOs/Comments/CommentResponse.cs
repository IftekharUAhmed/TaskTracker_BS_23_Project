using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.DTOs.Comments
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }
        public int AuthorUserId { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
