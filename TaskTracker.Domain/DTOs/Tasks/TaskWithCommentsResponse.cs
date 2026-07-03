using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Domain.DTOs.Tasks
{
    public class TaskWithCommentsResponse : TaskItemResponse
    {
        public IEnumerable<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
    }
}
