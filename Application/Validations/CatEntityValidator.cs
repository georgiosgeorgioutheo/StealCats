using Core.Entities;
using FluentValidation;


namespace Application.Validations
{
    public class CatEntityValidator : AbstractValidator<CatEntity>
    {
        public CatEntityValidator()
        {
            RuleFor(cat => cat.CatId)
                .NotEmpty().WithMessage("CatId is required.")
                .Must(id => !string.IsNullOrWhiteSpace(id)).WithMessage("CatId cannot be whitespace only.");

            RuleFor(cat => cat.Width)
                .GreaterThan(0).WithMessage("Width must be greater than 0.");

            RuleFor(cat => cat.Height)
                .GreaterThan(0).WithMessage("Height must be greater than 0.");

            RuleFor(cat => cat.Image)
                .NotNull().WithMessage("Image is required.")
                .Must(image => image.Length > 0).WithMessage("Image cannot be empty.");

            RuleFor(cat => cat.Created)
                .NotEmpty().WithMessage("Created date is required.");

            RuleFor(cat => cat.CatTags)
                .NotNull().WithMessage("CatTags list cannot be null.");
           
        }
    }
}