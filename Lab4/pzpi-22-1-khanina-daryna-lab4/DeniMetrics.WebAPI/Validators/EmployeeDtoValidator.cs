using DineMetrics.Core.Dto;
using FluentValidation;

namespace DeniMetrics.WebAPI.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Position)
            .NotEmpty()
            .WithMessage("Position is required.")
            .MaximumLength(50)
            .WithMessage("Position cannot exceed 50 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number format is invalid.");

        RuleFor(x => x.WorkStart)
            .NotEmpty()
            .WithMessage("WorkStart date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("WorkStart date cannot be in the future.");

        RuleFor(x => x.WorkEnd)
            .GreaterThan(x => x.WorkStart)
            .When(x => x.WorkEnd.HasValue)
            .WithMessage("WorkEnd date must be after WorkStart date.");
    }
}
