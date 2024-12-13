using FluentValidation;
using System.Xml.Linq;

namespace GotExplorer.BLL.Validators.Extensions
{
    public static class FileValidationExtensions
    {
        public static IRuleBuilderOptions<T, IFormFile> HasValidFileSignature<T>(
            this IRuleBuilder<T, IFormFile> ruleBuilder, 
            IDictionary<string, IList<byte[]>> allowedSignatures)
        {
            return ruleBuilder.Must(file => 
            {
                using (var reader = new BinaryReader(file.OpenReadStream()))
                {
                    
                    var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

                    if (string.IsNullOrEmpty(fileExtension) || !allowedSignatures.ContainsKey(fileExtension))
                    {
                        return false;
                    }

                    var signatures = allowedSignatures[fileExtension];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    return signatures.Any(signature =>
                        headerBytes.Take(signature.Length).SequenceEqual(signature));
                }
            });
        }
    }
}
