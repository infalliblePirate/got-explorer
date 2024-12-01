using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Results
{
    public class ServiceResult<T>
    {
        public T? ResultObject { get; set; }
        public Error Error { get; set; } = Error.None;
        public bool IsSuccess
        {
            get
            {
                return (Error.ValidationResult == null) || (Error.ValidationResult != null && Error.ValidationResult.IsValid);
            }
        }
    }
}
