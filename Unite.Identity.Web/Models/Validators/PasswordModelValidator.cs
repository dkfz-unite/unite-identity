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

public class ResetPasswordRequestModelValidator : AbstractValidator<ResetPasswordRequestModel>
{
    public ResetPasswordRequestModelValidator()
    {
        RuleFor(model => model.Email)
            .NotEmpty().WithMessage("Should not be empty")
            .EmailAddress().WithMessage("Should be a valid email address")
            .MaximumLength(100).WithMessage("Maximum length is 100");
    }
}

public class ResetPasswordConfirmationModelValidator : AbstractValidator<ResetPasswordConfirmationModel>
{
    private readonly IValidator<string> _passwordValidator;

    public ResetPasswordConfirmationModelValidator()
    {
        _passwordValidator = new PasswordValidator();

        RuleFor(model => model.Token)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(model => model.Password)
            .SetValidator(_passwordValidator);

        RuleFor(model => model.PasswordRepeat)
            .SetValidator(_passwordValidator);

        RuleFor(model => model)
            .Must(HaveMatchedPasswords).WithMessage("Passwords should match");
    }

    private bool HaveMatchedPasswords(ResetPasswordConfirmationModel model)
    {
        return string.Equals(model.Password, model.PasswordRepeat);
    }
}
