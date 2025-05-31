using DineMetrics.Core.Dto;
using FluentValidation;

namespace DeniMetrics.WebAPI.Validators;

public class DeviceDtoValidator : AbstractValidator<DeviceDto>
{
    public DeviceDtoValidator()
    {
        RuleFor(x => x.SerialNumber)
            .NotEmpty()
            .WithMessage("SerialNumber is required.")
            .MaximumLength(50)
            .WithMessage("SerialNumber cannot exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required.")
            .MaximumLength(100)
            .WithMessage("Model cannot exceed 100 characters.");
    }
}