using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Domain.Validators.Tasks
{
    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid task status.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority level.");
        }
    }
}
