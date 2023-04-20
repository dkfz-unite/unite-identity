using FluentValidation;

namespace Unite.Identity.Models.Validators;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    private readonly IValidator<string> _passwordValidator;

    public RegisterModelValidator()
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

    private bool HaveMatchedPasswords(RegisterModel model)
    {
        return string.Equals(model.Password, model.PasswordRepeat);
    }
}