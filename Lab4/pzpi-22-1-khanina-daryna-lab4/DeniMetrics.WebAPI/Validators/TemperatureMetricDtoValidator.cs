using DineMetrics.Core.Dto;
using FluentValidation;

namespace DeniMetrics.WebAPI.Validators;

public class TemperatureMetricDtoValidator : AbstractValidator<TemperatureMetricDto>
{
    public TemperatureMetricDtoValidator()
    {
        RuleFor(x => x.Value)
            .InclusiveBetween(-100, 100)
            .WithMessage("Temperature value must be between -100 and 100.");

        RuleFor(x => x.Time)
            .NotEmpty()
            .WithMessage("Time is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Time cannot be in the future.");
    }
}