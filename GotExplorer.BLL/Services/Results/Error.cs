using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Results
{
    public class Error
    {
        public string Code { get; set; }
        public ValidationResult? ValidationResult { get; set; }
        public Error(string code, ValidationResult? validationResult)
        {
            Code = code;
            ValidationResult = validationResult;
        }

        public static readonly Error None = new(ErrorCodes.None, null);
    }
}
