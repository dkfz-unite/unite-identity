using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class CreateAccountModelValidator : AbstractValidator<CreateAccountModel>
{
    private readonly IValidator<string> _passwordValidator;

    public CreateAccountModelValidator()
    {
        _passwordValidator = new PasswordValidator();

        RuleFor(model => model.Email)
            .NotEmpty().WithMessage("Should not be empty")
            .EmailAddress().WithMessage("Should be an email address")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(model => model.Password)
            .SetValidator(_passwordValidator);

        RuleFor(model => model.PasswordRepeat)
            .SetValidator(_passwordValidator);

        RuleFor(model => model)
            .Must(HaveMatchedPasswords).WithMessage("Passwords should match");
    }

    private bool HaveMatchedPasswords(CreateAccountModel model)
    {
        return string.Equals(model.Password, model.PasswordRepeat);
    }
}