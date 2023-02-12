using Microsoft.AspNetCore.Mvc;
using SampleAPI.Handlers.Models;
using SampleAPI.Handlers;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        private readonly IHandler<string, StatisticsResponse> getStatisticsHandler;
        private readonly IHandler<string, bool> resetStatisticsHandler;

        public StatsController(
            IHandler<string, StatisticsResponse> getStatisticsHandler,
            IHandler<string, bool> resetStatisticsHandler)
        {
            this.getStatisticsHandler = getStatisticsHandler;
            this.resetStatisticsHandler = resetStatisticsHandler;
        }

        [HttpGet]
        [Route("")]
        public async Task<StatisticsResponse> GetStatistics(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return await getStatisticsHandler.Execute(HttpContext.Connection.Id, token);
        }

        [HttpPost]
        [Route("reset")]
        public async Task<ActionResult> ResetStatistics(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await resetStatisticsHandler.Execute(HttpContext.Connection.Id, token);
            return  Ok();
        }
    }
}
