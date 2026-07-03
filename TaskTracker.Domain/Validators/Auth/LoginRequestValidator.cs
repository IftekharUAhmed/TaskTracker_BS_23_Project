using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Domain.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
