using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators.Extensions;
using Microsoft.Extensions.Options;

namespace GotExplorer.BLL.Validators
{
    public class Model3dValidator : AbstractValidator<UploadModel3dDTO>
    {
        private static IDictionary<string, IList<byte[]>> _modelSignatures = new Dictionary<string, IList<byte[]>>()
        {
            {
                ".glb", new List<byte[]>()
                {
                    new byte[] { 0x46, 0x4F, 0x52, 0x4D },
                    new byte[] { 0x67, 0x6C, 0x54, 0x46, 0x01, 0x00, 0x00, 0x00 },
                    new byte[] { 0x67, 0x6C, 0x54, 0x46, 0x02, 0x00, 0x00, 0x00 },
                    new byte[] { 0x47, 0x4C, 0x42, 0x52 },
                    new byte[] { 0x64, 0x9B, 0xD1, 0x09 },
                    new byte[] { 0x48, 0x4C, 0x49, 0x52 },
                    new byte[] { 0x00, 0x47, 0x4C, 0x42 }
                }
            }
        };

        private readonly UploadFileLimitOptions _options;
        public Model3dValidator(IOptions<UploadFileLimitOptions> options) 
        {
            _options = options.Value;

            RuleFor(x => x.Model.Length)
                .Must(x => x > 0)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.FileCannotBeEmpty)
                .Must(x => x < _options.MaxModel3dSize)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.FileSizeShouldBeLessThan, "File", _options.MaxModel3dSize / 1024 / 1024));
            RuleFor(x => x.Model)
                .HasValidFileSignature(_modelSignatures)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidModel);
        }
    }
}
