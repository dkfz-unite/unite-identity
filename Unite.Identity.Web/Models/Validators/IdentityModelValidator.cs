using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class IdentityModelValidator : AbstractValidator<IdentityModel>
{
    public IdentityModelValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Should not be empty");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Should not be empty");
    }
}