using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Validators.Users
{
    public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        public UpdateUserRoleRequestValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == UserRoles.Admin || role == UserRoles.Manager || role == UserRoles.Member)
                .WithMessage("Role must be Admin, Manager, or Member.");
        }
    }
}
