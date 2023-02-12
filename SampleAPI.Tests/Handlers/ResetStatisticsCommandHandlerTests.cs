using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SampleAPI.Enums;
using SampleAPI.Handlers.Commands;
using SampleAPI.Handlers.Models;
using SampleAPI.Handlers;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.ExternalClients.Models;
using SampleAPI.Services.ExternalClients;
using SampleAPI.Services.ResultProvider;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class ResetStatisticsCommandHandlerTests
    {
        private Mock<IUserStatisticsProvider> statisticsProvider;
        private IHandler<string, bool> handler;

        [TestInitialize]
        public void SetUp()
        {
            statisticsProvider = new Mock<IUserStatisticsProvider>();
            statisticsProvider.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            handler = new ResetStatisticsCommandHandler(statisticsProvider.Object);

        }

        [TestMethod]
        public async Task Execute_Success()
        {
            // Act
            var response = await handler.Execute("test", CancellationToken.None);

            // Assert
            Assert.IsTrue(response);
            statisticsProvider.Verify(x => x.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Execute_Cancelled()
        {
            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await handler.Execute("test", new CancellationToken(true));
            });
        }
    }
}
