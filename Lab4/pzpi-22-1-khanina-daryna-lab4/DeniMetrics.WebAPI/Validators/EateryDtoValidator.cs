using DineMetrics.Core.Dto;
using FluentValidation;

namespace DeniMetrics.WebAPI.Validators;

public class EateryDtoValidator : AbstractValidator<EateryDto>
{
    public EateryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters long.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address must be at most 200 characters long.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type must be a valid EateryType.");

        RuleFor(x => x.OpeningDay)
            .NotEmpty().WithMessage("OpeningDay is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("OpeningDay cannot be in the future.");
    }
}