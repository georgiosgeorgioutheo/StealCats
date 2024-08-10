using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class CatRequestValidator : AbstractValidator<(int page, int pageSize)>
    {
        public CatRequestValidator()
        {
            RuleFor(request => request.page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(request => request.pageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100."); // Optional limit
        }
    }
}
