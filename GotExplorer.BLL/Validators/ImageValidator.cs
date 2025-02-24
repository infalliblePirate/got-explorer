using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators.Extensions;
using Microsoft.Extensions.Options;
using System.Linq;

namespace GotExplorer.BLL.Validators
{
    public class ImageValidator : AbstractValidator<UploadImageDTO>
    {
        private static IDictionary<string, IList<byte[]>> _imageSignatures = new Dictionary<string, IList<byte[]>>()
        {
            {  ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            },
            {
                ".jpeg", new List<byte[]>()
                {
                    new byte[] { 0xFF, 0xD8 },
                }
            },
            {
                ".jpg", new List<byte[]>()
                {
                    new byte[] { 0xFF, 0xD8 },
                }
            },
            {
                ".gif", new List<byte[]>()
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 },
                    new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 },
                }
            },
            {
                ".bmp", new List<byte[]>()
                {
                    new byte[] { 0x42, 0x4D },
                }
            }
        };
        private readonly UploadFileLimitOptions _options;
        public ImageValidator(IOptions<UploadFileLimitOptions> options)
        {
            _options = options.Value;

            RuleFor(x => x.Image.Length)
                .Must(x => x > 0)
                .WithMessage(ErrorMessages.FileCannotBeEmpty)
                .WithErrorCode(ErrorCodes.Invalid)
                .Must(x => x < _options.MaxImageSize)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.FileSizeShouldBeLessThan, _options.MaxImageSize / 1024 / 1024));
            RuleFor(x => x.Image)
                .HasValidFileSignature(_imageSignatures)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidImage);
        }
    }
}
