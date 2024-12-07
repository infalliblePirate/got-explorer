using FluentValidation;
using GotExplorer.BLL.DTOs;

namespace GotExplorer.BLL.Validators
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {       
            RuleFor(x => x.Length)
                .Must(x => x > 0)
                .WithMessage("File cannot be empty.");
        }
    }
}
