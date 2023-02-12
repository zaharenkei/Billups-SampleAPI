using SampleAPI.Enums;
using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Tests.Handlers
{
    [TestClass]
    public class StatisticsResponseModelTests
    {
        [TestMethod]
        public void Create_NoStats()
        {
            // Act
            var response = new StatisticsResponse(Array.Empty<RoundScore>());

            // Assert
            Assert.AreEqual(response.WinRate, "0%");
            Assert.AreEqual(response.Scores.Length,0);
        }

        [TestMethod]
        public void Create_Draw()
        {
            // Act
            var response = new StatisticsResponse(new []{new RoundScore(1,2,GameResult.Lose), new RoundScore(2,1,GameResult.Win)});

            // Assert
            Assert.AreEqual(response.WinRate, "50%");
            Assert.AreEqual(response.Score, "Player: 1|1 : Computer");
            Assert.AreEqual(response.Scores.Length, 2);
        }
    }
}
