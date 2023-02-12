using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SampleAPI.Enums;
using SampleAPI.Handlers.Queries;
using SampleAPI.Handlers;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders.Pagination;
using SampleAPI.Services.DataProviders;
using SampleAPI.Handlers.Models;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class GetStatisticsQueryHandlerTests
    {
        private Mock<IUserStatisticsProvider> dataProvider;
        private IHandler<string, StatisticsResponse> handler;

        [TestInitialize]
        public void SetUp()
        {
            dataProvider = new Mock<IUserStatisticsProvider>();
            dataProvider.Setup(x => x.Read(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new[] { new RoundScore(1, 2, GameResult.Win) });

            handler = new GetStatisticsQueryHandler(dataProvider.Object);

        }

        [TestMethod]
        public async Task Execute_Success()
        {
            // Act
            var response = await handler.Execute("Test", CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Scores.Length, 1);
            dataProvider.Verify(x => x.Read(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Execute_Cancelled()
        {
            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await handler.Execute("Test", new CancellationToken(true));
            });
        }
    }
}
