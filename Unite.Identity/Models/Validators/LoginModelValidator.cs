using FluentValidation;

namespace Unite.Identity.Models.Validators;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Should not be empty");
    }
}