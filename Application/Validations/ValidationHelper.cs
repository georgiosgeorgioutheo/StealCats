using Core.DTOs;
using Core.Entities;
using FluentValidation;

namespace Application.Validations
{
    public static  class ValidationHelper
    {
        private static readonly IValidator<PaginatedCatResponseDTO> _paginatedCatValidator = new PaginatedCatResponseDtoValidator();
        private static readonly IValidator<CatEntity> _catEntityValidator = new CatEntityValidator();
        private static readonly IValidator<CatResponseDto> _catResponseDtoValidator = new CatResponseDtoValidator();
        private static readonly IValidator<CatApiResponse> _catApiResponseValidator = new CatApiResponseValidator();
        private static readonly IValidator<(int page, int pageSize)> _paginationValidator = new CatRequestValidator();


        public static async Task ValidatePaginatedCatResponseDtoAsync(PaginatedCatResponseDTO paginatedCatResponse)
        {
            var validationResult = await _paginatedCatValidator.ValidateAsync(paginatedCatResponse);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("PaginatedCatResponseDTO validation failed.", validationResult.Errors);
            }
        }
        public static async Task ValidateCatEntityAsync(CatEntity cat)
        {
            var validationResult = await _catEntityValidator.ValidateAsync(cat);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("CatEntity validation failed.", validationResult.Errors);
            }
        }


        public static async Task ValidateCatEntitiesAsync(IEnumerable<CatEntity> cats)
        {
            foreach (var cat in cats)
            {
                await ValidateCatEntityAsync(cat);
            }
        }

        public static async Task ValidateCatResponseDtoAsync(CatResponseDto catResponseDto)
        {
            var validationResult = await _catResponseDtoValidator.ValidateAsync(catResponseDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException("CatResponseDto validation failed.", validationResult.Errors);
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
            var validationResult = await _catApiResponseValidator.ValidateAsync(catApiResponse);

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

        public static async Task ValidatePaginationParametersAsync(int page, int pageSize)
        {
            var validationResult = await _paginationValidator.ValidateAsync((page, pageSize));

            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation failed for pagination parameters.", validationResult.Errors);
            }
        }


        public static Task ValidateBaseUrlAsync(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));
            }

            if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                throw new InvalidOperationException("The constructed base URL is not a valid URI.");
            }

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

            var validMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!validMimeTypes.Contains(mimeType.ToLowerInvariant()))
            {
                throw new InvalidOperationException("Unsupported MIME type.");
            }
            return Task.CompletedTask;
        }
    }
}
