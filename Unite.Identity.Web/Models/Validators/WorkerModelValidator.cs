using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class AddWorkerModelValidator : AbstractValidator<AddWorkerModel>
{
    public AddWorkerModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MaximumLength(100).WithMessage("Maximum length is 100");
    }
}

public class AddWorkerTokenModelValidator : AbstractValidator<AddWorkerTokenModel>
{
    public AddWorkerTokenModelValidator()
    {
        RuleFor(model => model)
            .Must(HaveExpiryTime).WithMessage("Should have expiry time set");

        RuleFor(model => model.ExpiryMinutes)
            .Empty().When(model => model.ExpiryHours != null || model.ExpiryDays != null).WithMessage("Expiry time is already set")
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(59).WithMessage("Should be less than or equal to 59");

        RuleFor(model => model.ExpiryHours)
            .Empty().When(model => model.ExpiryMinutes != null || model.ExpiryDays != null).WithMessage("Expiry time is already set")
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(23).WithMessage("Should be less than or equal to 23");

        RuleFor(model => model.ExpiryDays)
            .Empty().When(model => model.ExpiryMinutes != null || model.ExpiryHours != null).WithMessage("Expiry time is already set")
            .GreaterThanOrEqualTo(1).WithMessage("Should be greater than or equal to 1")
            .LessThanOrEqualTo(365).WithMessage("Should be less than or equal to 365");
    }

    private static bool HaveExpiryTime(AddWorkerTokenModel model)
    {
        return model.ExpiryMinutes != null || model.ExpiryHours != null || model.ExpiryDays != null;
    }
}
