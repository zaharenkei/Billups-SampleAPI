using Microsoft.AspNetCore.Mvc;
using SampleAPI.Handlers;
using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class GameController : ControllerBase
    {
        private readonly IHandler<IEnumerable<Choice>> getChoicesHandler;
        private readonly IHandler<Choice> getChoiceHandler;
        private readonly IHandler<PlayGameRequest, PlayGameResponse> playHandler;

        public GameController(
            IHandler<IEnumerable<Choice>> getChoicesHandler,
            IHandler<Choice> getChoiceHandler,
            IHandler<PlayGameRequest, PlayGameResponse> playHandler)
        {
            this.getChoicesHandler = getChoicesHandler;
            this.getChoiceHandler = getChoiceHandler;
            this.playHandler = playHandler;
        }

        [HttpGet]
        [Route("choices")]
        public async Task<IEnumerable<Choice>> GetAvailableChoices(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await getChoicesHandler.Execute(token);
        }

        [HttpGet]
        [Route("choice")]
        public async Task<Choice> GetRandomChoice(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await getChoiceHandler.Execute(token);
        }

        [HttpPost]
        [Route("play")]
        public async Task<PlayGameResponse> PlayGame([FromBody] PlayGameRequest request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            request.ConnectionId = HttpContext.Connection.Id;
            return await playHandler.Execute(request, token);
        }
    }
}