using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class PageResponseDtoValidator : AbstractValidator<PageResponseDto>
    {
        public PageResponseDtoValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0.");
            RuleFor(x => x.PageCount).GreaterThanOrEqualTo(0).WithMessage("Page count must be 0 or greater.");
            RuleFor(x => x.TotalCount).GreaterThanOrEqualTo(0).WithMessage("Total count must be 0 or greater.");
            RuleFor(x => x.CurrentPage).GreaterThan(0).WithMessage("Current page must be greater than 0.");
            RuleFor(x => x.PageInfo).NotEmpty().WithMessage("Page info is required.");
        }
    }

    public class PaginatedCatResponseDtoValidator : AbstractValidator<PaginatedCatResponseDTO>
    {
        public PaginatedCatResponseDtoValidator()
        {
            RuleFor(x => x.pageResponseDto).NotNull().SetValidator(new PageResponseDtoValidator());
            RuleFor(x => x.CatResponses).NotNull().NotEmpty().WithMessage("Cat responses cannot be null or empty.");
        }
    }
}
