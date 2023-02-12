using FluentValidation;
using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders;

namespace SampleAPI.Validators
{
    public class GameRequestValidator : AbstractValidator<PlayGameRequest>
    {
        public GameRequestValidator(IGameChoicesProvider choiceDataProvider)
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Player)
                .MustAsync(async (value, token) => await choiceDataProvider.Exist(value, token))
                .WithMessage((_, value) => $"Entity with Id:{value} could not be found.");
        }
    }
}
