using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Results
{
    public class ValidationWithEntityModel<TData> : ValidationResult
    {
        public TData? Entity { get; set; }
        
        public ValidationWithEntityModel(TData entity) : base() 
        {
            Entity = entity;
        }

        public ValidationWithEntityModel(ValidationResult validationResult) : base()
        {
            Errors = validationResult.Errors;
            RuleSetsExecuted = validationResult.RuleSetsExecuted;
        }

        public ValidationWithEntityModel(List<ValidationFailure> errors) : base()
        {
            Errors = errors;
        }

        public ValidationWithEntityModel(ValidationFailure error) : base()
        {
            Errors.Add(error);
        }
    }
}
