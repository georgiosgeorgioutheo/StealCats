using Application.Validations;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

public class CatRequestValidatorTests
{
    private readonly CatRequestValidator _validator;

    public CatRequestValidatorTests()
    {
        _validator = new CatRequestValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Invalid(int invalidPage)
    {
        var result = _validator.TestValidate((invalidPage, pageSize: 10));
        result.ShouldHaveValidationErrorFor(x => x.Item1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1001)] // Assuming 1000 is the maximum allowed pageSize
    public void Should_Have_Error_When_PageSize_Is_Invalid(int invalidPageSize)
    {
        var result = _validator.TestValidate((page: 1, invalidPageSize));
        result.ShouldHaveValidationErrorFor(x => x.Item2);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Page_And_PageSize_Are_Valid()
    {
        var result = _validator.TestValidate((page: 1, pageSize: 10));
        result.ShouldNotHaveValidationErrorFor(x => x.page);
        result.ShouldNotHaveValidationErrorFor(x => x.pageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Too_Small()
    {
        var result = _validator.TestValidate((page: 1, pageSize: 0)); // Assuming pageSize cannot be less than 1
        result.ShouldHaveValidationErrorFor(x => x.pageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Too_Large()
    {
        var result = _validator.TestValidate((page: 1, pageSize: 1001)); // Assuming 1000 is the maximum allowed pageSize
        result.ShouldHaveValidationErrorFor(x => x.pageSize);
    }

    [Fact]
    public void Should_Not_Have_Error_When_PageSize_Is_Within_Valid_Range()
    {
        var result = _validator.TestValidate((page: 1, pageSize: 50)); // Assuming pageSize between 1 and 1000 is valid
        result.ShouldNotHaveValidationErrorFor(x => x.pageSize);
    }
}