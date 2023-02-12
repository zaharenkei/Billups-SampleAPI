using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SampleAPI.Enums;
using SampleAPI.Handlers;
using SampleAPI.Handlers.Commands;
using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.ExternalClients;
using SampleAPI.Services.ExternalClients.Models;
using SampleAPI.Services.ResultProvider;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class PlayGameCommandHandlerTests
    {
        private PlayGameRequest defaultRequest;
        private RoundScore defaultScore;
        
        private Mock<IUserStatisticsProvider> statisticsProvider;
        private IHandler<PlayGameRequest, PlayGameResponse> handler;

        [TestInitialize]
        public void SetUp()
        {
            defaultRequest = new PlayGameRequest { Player = 1, ConnectionId = "Test1" };
            defaultScore = new RoundScore(defaultRequest.Player, 3, GameResult.Win);

            var client = new Mock<IRandomClient>();
            client.Setup(x => x.GetRandomNumber(It.IsAny<CancellationToken>())).ReturnsAsync(new RandomResponse { Value = 2 });

            var dataProvider = new Mock<IGameChoicesProvider>();
            dataProvider.Setup(x => x.Count(It.IsAny<CancellationToken>())).ReturnsAsync(5);

            var resultProvider = new Mock<IResultProvider>();
            resultProvider.Setup(x => x.DefineWinner(It.IsAny<int>(), It.IsAny<int>())).Returns(defaultScore);

            statisticsProvider = new Mock<IUserStatisticsProvider>();
            statisticsProvider.Setup(x => x.CreateOrUpdate(It.IsAny<string>(), It.IsAny<RoundScore>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { defaultScore });

            handler = new PlayGameCommandHandler(dataProvider.Object, client.Object, resultProvider.Object, statisticsProvider.Object, NullLogger<PlayGameCommandHandler>.Instance);

        }

        [TestMethod]
        public async Task Execute_Success()
        {
            // Act
            var response = await handler.Execute(defaultRequest, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.PlayerChoice, (int)defaultScore.PlayerChoice);
            Assert.AreEqual(response.ComputerChoice, (int)defaultScore.ComputerChoice);
            Assert.AreEqual(response.Result, defaultScore.Result.ToString());
        }

        [TestMethod]
        public async Task Execute_Cancelled()
        {
            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await handler.Execute(defaultRequest, new CancellationToken(true));
            });
        }

        [TestMethod]
        public async Task Execute_NoConnectionId_NoStatisticsRecorded()
        {
            // Arrange
            var request = new PlayGameRequest { Player = 1};

            // Act
            var response = await handler.Execute(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.PlayerChoice, (int)defaultScore.PlayerChoice);
            Assert.AreEqual(response.ComputerChoice, (int)defaultScore.ComputerChoice);
            Assert.AreEqual(response.Result, defaultScore.Result.ToString());
            statisticsProvider.Verify(x => x.CreateOrUpdate(It.IsAny<string>(), It.IsAny<RoundScore>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
