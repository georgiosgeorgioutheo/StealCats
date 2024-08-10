using Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .NotEmpty().WithMessage("Created date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future.");

            RuleFor(cat => cat.CatTags)
                .NotNull().WithMessage("CatTags list cannot be null.");
            // Add further validation if you want to ensure certain properties of the tags
        }
    }
}