using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class AddTokenModelValidator : AbstractValidator<AddTokenModel>
{
    public AddTokenModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.ExpiryDate)
            .SetValidator(new ExpiryDateModelValidator());
    }
}

public class ExpiryDateModelValidator : AbstractValidator<EpiryDateModel>
{
    public ExpiryDateModelValidator()
    {
        RuleFor(model => model)
            .Must(HaveExpiryTime).WithMessage("Should have expiry time set");

        RuleFor(model => model.ExpiryMinutes)
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(59).WithMessage("Should be less than or equal to 59");

        RuleFor(model => model.ExpiryHours)
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(23).WithMessage("Should be less than or equal to 23");

        RuleFor(model => model.ExpiryDays)
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(365).WithMessage("Should be less than or equal to 365");
    }

    private static bool HaveExpiryTime(EpiryDateModel model)
    {
        return model.ExpiryMinutes != null || model.ExpiryHours != null || model.ExpiryDays != null;
    }
}
