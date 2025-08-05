using FluentValidation;

namespace Unite.Identity.Web.Models.Validators;

public class EditTokenModelValidator : AbstractValidator<EditTokenModel>
{
    public EditTokenModelValidator()
    {
        RuleFor(model => model.ExpiryDate)
            .SetValidator(new ExpiryDateModelValidator());
    }
}
