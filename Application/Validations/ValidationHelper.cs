using Core.DTOs;
using Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public static  class ValidationHelper
    {
        public static async Task ValidateCatEntityAsync(CatEntity cat)
        {
            var validator = new CatEntityValidator();
            var validationResult =await validator.ValidateAsync(cat);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("CatEntity validation failed.", validationResult.Errors);
            }
        }
        public static async Task ValidateCatEntities(IEnumerable<CatEntity> cats)
        {
            foreach (var cat in cats)
            {
               await ValidateCatEntityAsync(cat);
            }
        }

        public static async Task ValidateCatResponseDtoAsync(CatResponseDto catResponseDto)
        {
            var validator = new CatResponseDtoValidator();
            var validationResult = await validator.ValidateAsync(catResponseDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("CatEntity validation failed.", validationResult.Errors);
            }
        }
        public static async Task ValidateCatResponseDtoListAsync(IEnumerable<CatResponseDto> catResponseDtos)
        {
            foreach (var catResponseDto in catResponseDtos)
            {
                await ValidateCatResponseDtoAsync(catResponseDto);
            }
        }

        public static async Task ValidateCatApiResponseAsync(CatApiResponse catApiResponse)
        {
            var validator = new CatApiResponseValidator();
            var validationResult = await validator.ValidateAsync(catApiResponse);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("CatApiResponse validation failed.", validationResult.Errors);
            }
        }

        public static async Task ValidateCatApiResponseListAsync(List<CatApiResponse> catApiResponses)
        {
            foreach (var catApiResponse in catApiResponses)
            {
              await ValidateCatApiResponseAsync(catApiResponse);
            }
        }

        public static async Task ValidatePaginationParameters(int page, int pageSize)
        {
            var validator = new CatRequestValidator();
            var validationResult = await validator.ValidateAsync((page, pageSize));

            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed for pagination parameters.", validationResult.Errors);
            }
        }

        public static  Task ValidateBaseUrlAsync(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));
            }

            if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                throw new InvalidOperationException("The constructed base URL is not a valid URI.");
            }

            // You could add more validation logic here if needed.

            return Task.CompletedTask;
        }

        public static Task ValidateImageFile(byte[] image, string mimeType)
        {
            if (image == null || image.Length == 0)
            {
                throw new InvalidOperationException("Image data is either null or empty.");
            }

            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentException("MIME type cannot be null or empty.", nameof(mimeType));
            }

            // Optionally, you could validate that the mimeType is a recognized image type.
            var validMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!validMimeTypes.Contains(mimeType.ToLowerInvariant()))
            {
                throw new InvalidOperationException("Unsupported MIME type.");
            }
            return Task.CompletedTask;
        }
    }
}
