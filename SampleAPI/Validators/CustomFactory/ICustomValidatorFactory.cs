using FluentValidation;

namespace SampleAPI.Validators.CustomFactory;

public interface ICustomValidatorFactory
{
    IValidator GetValidatorFor(Type type);
}