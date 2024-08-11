using Core.DTOs;
using FluentValidation;


public class CatResponseDtoValidator : AbstractValidator<CatResponseDto>
{
    public CatResponseDtoValidator()
    {
        RuleFor(cat => cat.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(cat => cat.CatId)
            .NotEmpty().WithMessage("CatId is required.")
            .Must(id => !string.IsNullOrWhiteSpace(id)).WithMessage("CatId cannot be whitespace only.");

        RuleFor(cat => cat.Width)
            .GreaterThan(0).WithMessage("Width must be greater than 0.");

        RuleFor(cat => cat.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0.");

        RuleFor(cat => cat.Created)
            .NotEmpty().WithMessage("Created date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future.");

        RuleFor(cat => cat.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .Must(BeAValidUrl).WithMessage("ImageUrl must be a valid URL.");

        RuleFor(cat => cat.Tags)
            .NotNull().WithMessage("Tags list cannot be null.")
            .Must(tags => tags.Count > 0).WithMessage("There must be at least one tag.")
            .ForEach(tag => tag.NotEmpty().WithMessage("Tag cannot be empty."));
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
