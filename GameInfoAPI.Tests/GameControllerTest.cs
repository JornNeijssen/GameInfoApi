using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using GameInfoAPI.Controllers;
using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameInfoAPI.Tests
{
    public class GameControllerTest
    {
        [Fact]
        public async Task GetAllGames_ShouldReturnAllGames()
        {
            // Arrange
            var testData = new List<Game>
            {
                new Game { GameId = 1, GameTitle = "Game 1" },
                new Game { GameId = 2, GameTitle = "Game 2" }
            };

            // Mock DbContext
            var dbContext = A.Fake<DataContext>();

            // Setup voor de mock DbContext om ToListAsync() te mocken
            //A.CallTo(() => dbContext.Games.Include(g => g.Players).Include(g => g.Author).ToListAsync())
            //    .Returns(testData);

            // Controller instantiëren met de mock DbContext
            var controller = new GameController(dbContext);

            // Act
            var result = await controller.GetAllGames();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var games = Assert.IsAssignableFrom<List<Game>>(okResult.Value);
            Assert.Equal(2, games.Count); // Controleren of twee games worden geretourneerd
            Assert.Equal("Game 1", games[0].GameTitle); // Controleren of de eerste game correct is
            Assert.Equal("Game 2", games[1].GameTitle); // Controleren of de tweede game correct is
        }
    }
}