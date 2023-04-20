using FluentValidation;

namespace Unite.Identity.Models.Validators;

public class PasswordChangeModelValidator : AbstractValidator<PasswordChangeModel>
{
    private readonly IValidator<string> _passwordValidator;

    public PasswordChangeModelValidator()
    {
        _passwordValidator = new PasswordValidator();

        RuleFor(model => model.OldPassword)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(model => model.NewPassword)
            .SetValidator(_passwordValidator);

        RuleFor(model => model.NewPasswordRepeat)
            .SetValidator(_passwordValidator);

        RuleFor(model => model)
            .Must(HaveMatchedPasswords).WithMessage("Passwords should match");
    }

    private bool HaveMatchedPasswords(PasswordChangeModel model)
    {
        return string.Equals(model.NewPassword, model.NewPasswordRepeat);
    }
}