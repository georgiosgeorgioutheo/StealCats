using Application.Validations;
using Core.DTOs;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

public class CatApiResponseValidatorTests
{
    private readonly CatApiResponseValidator _validator;

    public CatApiResponseValidatorTests()
    {
        _validator = new CatApiResponseValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var model = new CatApiResponse { Id = string.Empty, Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Id);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Width);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Height);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Url);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Width_Is_Zero_Or_Less()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 0, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Width_Is_Valid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Have_Error_When_Height_Is_Zero_Or_Less()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 0, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Height_Is_Valid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Empty()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = string.Empty };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Url);
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Invalid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "invalid-url" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Url)
            .WithErrorMessage("Url must be a valid URL.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Url_Is_Valid()
    {
        // Arrange
        var model = new CatApiResponse { Id = "valid-id", Width = 500, Height = 500, Url = "http://example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Url);
    }
}
