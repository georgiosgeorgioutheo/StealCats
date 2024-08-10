using Application.Validations;
using Core.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

public class CatEntityValidatorTests
{
    private readonly CatEntityValidator _validator;

    public CatEntityValidatorTests()
    {
        _validator = new CatEntityValidator();
    }

    [Fact]
    public void Should_Have_Error_When_CatId_Is_Empty()
    {
        var model = new CatEntity { CatId = string.Empty, Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.CatId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var model = new CatEntity 
        { CatId = "valid-id", 
            Width = 500,
            Height = 500, 
            Image = new byte[] { 1, 2, 3 }, 
            Created = DateTime.UtcNow.AddHours(-6) };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.CatId);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Width);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Height);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Image);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Created);
    }

    [Fact]
    public void Should_Have_Error_When_Width_Is_Zero_Or_Less()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 0, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Width_Is_Valid()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Width);
    }

    [Fact]
    public void Should_Have_Error_When_Height_Is_Zero_Or_Less()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 0, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Height_Is_Valid()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Height);
    }

    [Fact]
    public void Should_Have_Error_When_Image_Is_Empty()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = Array.Empty<byte>(), Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Image);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Image_Is_Valid()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Image);
    }

    [Fact]
    public void Should_Have_Error_When_Created_Is_Default()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = default(DateTime) };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(cat => cat.Created);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Created_Is_Valid()
    {
        var model = new CatEntity { CatId = "valid-id", Width = 500, Height = 500, Image = new byte[] { 1, 2, 3 }, Created = DateTime.UtcNow.AddHours(-6) };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(cat => cat.Created);
    }
}