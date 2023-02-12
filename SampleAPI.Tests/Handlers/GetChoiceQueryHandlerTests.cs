using Moq;
using SampleAPI.Enums;
using SampleAPI.Handlers;
using SampleAPI.Handlers.Queries;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.ExternalClients.Models;
using SampleAPI.Services.ExternalClients;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class GetChoiceQueryHandlerTests
    {
        private IHandler<Choice> handler;

        [TestInitialize]
        public void SetUp()
        {
            var client = new Mock<IRandomClient>();
            client.Setup(x => x.GetRandomNumber(It.IsAny<CancellationToken>())).ReturnsAsync(new RandomResponse { Value = 2 });

            var dataProvider = new Mock<IGameChoicesProvider>();
            dataProvider.Setup(x => x.Count(It.IsAny<CancellationToken>())).ReturnsAsync(5);
            dataProvider.Setup(x => x.Read(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Choice(GameChoice.Scissors));

            handler = new GetChoiceQueryHandler(dataProvider.Object, client.Object);

        }

        [TestMethod]
        public async Task Execute_Success()
        {
            // Act
            var response = await handler.Execute(CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task Execute_Cancelled()
        {
            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await handler.Execute(new CancellationToken(true));
            });
        }
    }
}
