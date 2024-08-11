using Core.DTOs;
using FluentValidation;


namespace Application.Validations
{
    public class CatApiResponseValidator : AbstractValidator<CatApiResponse>
    {
        public CatApiResponseValidator()
        {
            RuleFor(cat => cat.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(cat => cat.Width)
                .GreaterThan(0).WithMessage("Width must be greater than 0.");

            RuleFor(cat => cat.Height)
                .GreaterThan(0).WithMessage("Height must be greater than 0.");

            RuleFor(cat => cat.Url)
                .NotEmpty().WithMessage("Url is required.")
                .Must(BeAValidUrl).WithMessage("Url must be a valid URL.");

            RuleForEach(cat => cat.Breeds)
                .SetValidator(new BreedValidator());
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }

    public class BreedValidator : AbstractValidator<Breed>
    {
        public BreedValidator()
        {
            RuleFor(breed => breed.Temperament)
                .NotEmpty().WithMessage("Temperament is required.");
        }
    }
}
