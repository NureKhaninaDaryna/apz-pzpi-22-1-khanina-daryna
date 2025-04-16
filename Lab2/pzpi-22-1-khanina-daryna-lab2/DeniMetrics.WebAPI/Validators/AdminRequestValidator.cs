using DeniMetrics.WebAPI.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace DeniMetrics.WebAPI.Validators;

public class AdminRequestValidator : AbstractValidator<AdminRequest>
{
    public AdminRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");
    }
}