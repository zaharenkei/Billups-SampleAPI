using FluentValidation;

namespace SampleAPI.Validators.CustomFactory
{
    public class CustomValidatorFactory : ICustomValidatorFactory
    {
        private readonly IServiceProvider serviceProvider;

        public CustomValidatorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IValidator GetValidatorFor(Type type)
        {
            var genericValidatorType = typeof(IValidator<>);
            var specificValidatorType = genericValidatorType.MakeGenericType(type);
            using var scope = serviceProvider.CreateScope();
            var validatorInstance = scope.ServiceProvider.GetService(specificValidatorType);
            return (IValidator)validatorInstance!;
        }
    }
}
