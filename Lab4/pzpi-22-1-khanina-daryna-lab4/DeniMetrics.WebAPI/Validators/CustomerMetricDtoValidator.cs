using DineMetrics.Core.Dto;
using FluentValidation;

namespace DeniMetrics.WebAPI.Validators;

public class CustomerMetricDtoValidator : AbstractValidator<CustomerMetricDto>
{
    public CustomerMetricDtoValidator()
    {
        RuleFor(x => x.Count)
            .NotEqual(0)
            .WithMessage("Count must not be 0");
        
        RuleFor(x => x.Count)
            .GreaterThan(-15)
            .WithMessage("Count must be greater or equal to 15");
        
        RuleFor(x => x.Count)
            .LessThan(15)
            .WithMessage("Count must be less than 15");

        RuleFor(x => x.Time)
            .NotEmpty()
            .WithMessage("Time is required.");
    }
}