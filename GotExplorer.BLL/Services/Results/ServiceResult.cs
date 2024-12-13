using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Results
{
    public class ServiceResult<T> : ServiceResult
    {
        public T? ResultObject { get; set; }

        public static ServiceResult<T> Success(T? obj) 
        {
            return new ServiceResult<T> { ResultObject = obj };
        }
        public static ServiceResult<T> Failure(ValidationResult? result) 
        {
            return new ServiceResult<T> { ValidationResult = result };
        }
    }

    public class ServiceResult
    {
        public ValidationResult? ValidationResult { get; set; } = null;
        public bool IsSuccess
        {
            get
            {
                return (ValidationResult == null) || (ValidationResult != null && ValidationResult.IsValid);
            }
        }

        public static ServiceResult Success()
        {
            return new ServiceResult();
        }
        public static ServiceResult Failure(ValidationResult? result)
        {
            return new ServiceResult { ValidationResult = result };
        }
    }
}
