using Application.Validations;
using Core.DTOs;
using FluentValidation.TestHelper;
using Xunit;

public class PaginatedCatResponseDtoValidatorTests
{
    private readonly PaginatedCatResponseDtoValidator _validator;

    public PaginatedCatResponseDtoValidatorTests()
    {
        _validator = new PaginatedCatResponseDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PageResponseDto_Is_Null()
    {
        var model = new PaginatedCatResponseDTO { pageResponseDto = null, CatResponses = new List<CatResponseDto> { new CatResponseDto() } };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.pageResponseDto);
    }

    [Fact]
    public void Should_Have_Error_When_CatResponses_Is_Null()
    {
        var model = new PaginatedCatResponseDTO { pageResponseDto = new PageResponseDto(), CatResponses = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CatResponses).WithErrorMessage("Cat responses cannot be null or empty.");
    }

    [Fact]
    public void Should_Have_Error_When_CatResponses_Is_Empty()
    {
        var model = new PaginatedCatResponseDTO { pageResponseDto = new PageResponseDto(), CatResponses = new List<CatResponseDto>() };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CatResponses).WithErrorMessage("Cat responses cannot be null or empty.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var model = new PaginatedCatResponseDTO
        {
            pageResponseDto = new PageResponseDto { PageSize = 10, PageCount = 1, TotalCount = 10, CurrentPage = 1, PageInfo = "Valid info" },
            CatResponses = new List<CatResponseDto> { new CatResponseDto() }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}