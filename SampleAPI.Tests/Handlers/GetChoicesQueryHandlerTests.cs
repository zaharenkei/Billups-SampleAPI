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
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.DataProviders.Pagination;
using SampleAPI.Services.ExternalClients.Models;
using SampleAPI.Services.ExternalClients;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class GetChoicesQueryHandlerTests
    {
        private PageRequest defaultPageRequest;
        private IEnumerable<Choice> defaultChoices;
        private Mock<IGameChoicesProvider> dataProvider;
        private IHandler<IEnumerable<Choice>> handler;

        [TestInitialize]
        public void SetUp()
        {
            defaultPageRequest = new PageRequest(0, 2);
            defaultChoices = new[] { new Choice(GameChoice.Paper), new Choice(GameChoice.Rock), new Choice(GameChoice.Spock) };
            var page = new Page<Choice>(defaultPageRequest, defaultChoices.Take(defaultPageRequest.Limit), 3);

            dataProvider = new Mock<IGameChoicesProvider>();
            dataProvider.Setup(x => x.ReadPage(It.IsAny<PageRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);

            handler = new GetChoicesQueryHandler(dataProvider.Object);

        }

        [TestMethod]
        public async Task Execute_Success()
        {
            // Act
            var response = await handler.Execute(CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count(), defaultPageRequest.Limit);
            dataProvider.Verify(x => x.ReadPage(It.IsAny<PageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
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
