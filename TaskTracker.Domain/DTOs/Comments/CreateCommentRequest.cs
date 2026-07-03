using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.DTOs.Comments
{
    public class CreateCommentRequest
    {
        public string Body { get; set; } = string.Empty;
    }
}
