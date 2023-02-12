using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Validators.CustomFactory;
using System.Net;

namespace SampleAPI.Infrastructure.Middleware
{
    public class ApiValidationFilter : IAsyncActionFilter
    {
        private readonly ICustomValidatorFactory _validatorFactory;

        public ApiValidationFilter(ICustomValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.Any())
            {
                await next();
                return;
            }

            var validationFailures = new List<ValidationFailure>();

            foreach (var actionArgument in context.ActionArguments)
            {
                var validationErrors = await GetValidationErrorsAsync(actionArgument.Value);
                validationFailures.AddRange(validationErrors);
            }

            if (!validationFailures.Any())
            {
                await next();
                return;
            }

            context.Result = new BadRequestObjectResult(GenerateResult(validationFailures));
        }

        private async Task<IEnumerable<ValidationFailure>> GetValidationErrorsAsync(object? value)
        {

            if (value == null)
            {
                return new[] { new ValidationFailure("", "instance is null") };
            }

            var validatorInstance = _validatorFactory.GetValidatorFor(value.GetType());
            if (validatorInstance == null)
            {
                return new List<ValidationFailure>();
            }

            var validationResult = await validatorInstance.ValidateAsync(new ValidationContext<object>(value));
            return validationResult.Errors ?? new List<ValidationFailure>();
        }

        public static ProblemDetails GenerateResult(IEnumerable<ValidationFailure> validationFailures)
        {
            var errors = validationFailures.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);

            var problemDetails = new ProblemDetails
            {
                Type = "ValidationError",
                Status = (int)(HttpStatusCode.BadRequest)
            };

            problemDetails.Extensions.Add("errors", errors);
            return problemDetails;
        }
    }
}
