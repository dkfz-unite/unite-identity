using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotEmpty().WithMessage("Should not be empty")
            .MinimumLength(8).WithMessage("Minimul length is 8")
            .Must(HaveLetter).WithMessage("Should have at least 1 letter")
            //.Must(HaveCapitalLetter).WithMessage("Should have at least 1 capital letter")
            .Must(HaveNumber).WithMessage("Should have at least 1 number");

    }

    private bool HaveLetter(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Any(character =>
                char.IsLetter(character)
            );
        }

        return false;
    }

    private bool HaveCapitalLetter(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Any(character =>
                char.IsLetter(character) &&
                char.IsUpper(character)
            );
        }

        return false;
    }

    private bool HaveNumber(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Any(character =>
                char.IsNumber(character)
            );
        }

        return false;
    }
}