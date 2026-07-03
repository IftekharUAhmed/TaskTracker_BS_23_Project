using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Domain.Validators.Comments
{
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Comment body is required.")
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}
