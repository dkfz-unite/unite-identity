using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{
    private readonly IValidator<string> _passwordValidator;

    public ChangePasswordModelValidator()
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

    private bool HaveMatchedPasswords(ChangePasswordModel model)
    {
        return string.Equals(model.NewPassword, model.NewPasswordRepeat);
    }
}