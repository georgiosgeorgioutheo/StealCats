using Application.Validations;
using Core.DTOs;
using FluentValidation.TestHelper;
using Xunit;

public class PageResponseDtoValidatorTests
{
    private readonly PageResponseDtoValidator _validator;

    public PageResponseDtoValidatorTests()
    {
        _validator = new PageResponseDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new PageResponseDto { PageSize = 0, PageCount = 1, TotalCount = 10, CurrentPage = 1, PageInfo = "Valid info" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageSize).WithErrorMessage("Page size must be greater than 0.");
    }

    [Fact]
    public void Should_Have_Error_When_PageCount_Is_Less_Than_Zero()
    {
        var model = new PageResponseDto { PageSize = 10, PageCount = -1, TotalCount = 10, CurrentPage = 1, PageInfo = "Valid info" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageCount).WithErrorMessage("Page count must be 0 or greater.");
    }

    [Fact]
    public void Should_Have_Error_When_TotalCount_Is_Less_Than_Zero()
    {
        var model = new PageResponseDto { PageSize = 10, PageCount = 1, TotalCount = -1, CurrentPage = 1, PageInfo = "Valid info" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.TotalCount).WithErrorMessage("Total count must be 0 or greater.");
    }

    [Fact]
    public void Should_Have_Error_When_CurrentPage_Is_Less_Than_Or_Equal_To_Zero()
    {
        var model = new PageResponseDto { PageSize = 10, PageCount = 1, TotalCount = 10, CurrentPage = 0, PageInfo = "Valid info" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CurrentPage).WithErrorMessage("Current page must be greater than 0.");
    }

    [Fact]
    public void Should_Have_Error_When_PageInfo_Is_Empty()
    {
        var model = new PageResponseDto { PageSize = 10, PageCount = 1, TotalCount = 10, CurrentPage = 1, PageInfo = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageInfo).WithErrorMessage("Page info is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var model = new PageResponseDto { PageSize = 10, PageCount = 1, TotalCount = 10, CurrentPage = 1, PageInfo = "Valid info" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}