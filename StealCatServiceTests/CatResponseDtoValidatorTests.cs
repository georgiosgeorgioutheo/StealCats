using Core.DTOs;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

public class CatResponseDtoValidatorTests
{
    private readonly CatResponseDtoValidator _validator;

    public CatResponseDtoValidatorTests()
    {
        _validator = new CatResponseDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_CatId_Is_Empty()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = string.Empty, // Invalid
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        var validator = new CatResponseDtoValidator();

        // Act & Assert
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.CatId);
    }

    [Fact]
    public void Should_Have_Error_When_Tags_Is_Null()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string>() // Invalid
        };

        var validator = new CatResponseDtoValidator();

        // Act & Assert
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Tags);
    }

    [Fact]
    public void Should_Have_Error_When_Width_Is_Zero_Or_Less()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 0, // Invalid
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Width_Is_Valid()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500, // Valid
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Have_Error_When_Height_Is_Zero_Or_Less()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 0, // Invalid
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Height_Is_Valid()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500, // Valid
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Have_Error_When_ImageUrl_Is_Empty()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = string.Empty, // Invalid
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.ImageUrl);
    }

    [Fact]
    public void Should_Have_Error_When_ImageUrl_Is_Invalid()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "invalid-url", // Invalid URL
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.ImageUrl)
            .WithErrorMessage("ImageUrl must be a valid URL.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_ImageUrl_Is_Valid()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com", // Valid URL
            Tags = new List<string> { "Tag1", "Tag2" }
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.ImageUrl);
    }

 

   

    [Fact]
    public void Should_Not_Have_Error_When_Tags_Is_Valid()
    {
        // Arrange
        var model = new CatResponseDto
        {
            CatId = "valid-id",
            Width = 500,
            Height = 500,
            Created = DateTime.UtcNow,
            ImageUrl = "http://example.com",
            Tags = new List<string> { "Tag1", "Tag2" } // Valid
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Tags);
    }
}