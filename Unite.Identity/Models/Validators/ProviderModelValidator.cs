using System;
using FluentValidation;

namespace Unite.Identity.Models.Validators;

public class AddProviderModelValidator : AbstractValidator<AddProviderModel>
{
    public AddProviderModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.Label)
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.IsActive)
            .NotEmpty().WithMessage("Should not be empty");
    }
}

public class EditProviderModelValidator : AbstractValidator<EditProviderModel>
{
    public EditProviderModelValidator()
    {
        RuleFor(model => model.Id)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.Label)
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.IsActive)
            .NotEmpty().WithMessage("Should not be empty");
    }
}
