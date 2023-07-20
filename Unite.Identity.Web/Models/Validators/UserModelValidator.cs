using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class AddUserModelValidator : AbstractValidator<AddUserModel>
{
    public AddUserModelValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty().WithMessage("Should not be empty")
            .EmailAddress().WithMessage("Should be an email address")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.ProviderId)
            .NotEmpty().WithMessage("Should not be empty");
    }
}

public class EditUserModelValidator : AbstractValidator<EditUserModel>
{
    public EditUserModelValidator()
    {
        RuleFor(model => model.ProviderId)
            .NotEmpty().WithMessage("Should not be empty");
    }
}

public class CheckUserModelValidator : AbstractValidator<CheckUserModel>
{
    public CheckUserModelValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty().WithMessage("Should not be empty")
            .EmailAddress().WithMessage("Should be an email address")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.ProviderId)
            .NotEmpty().WithMessage("Should not be empty");
    }
}
