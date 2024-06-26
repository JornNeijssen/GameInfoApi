using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GameInfoAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GameInfoAPI.Tests.Integration
{
    public class GameControllerIntegrationTests : IClassFixture<WebApplicationFactory<GameInfoAPI.Program>>
    {
        private readonly WebApplicationFactory<GameInfoAPI.Program> _factory;

        public GameControllerIntegrationTests(WebApplicationFactory<GameInfoAPI.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetGames_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/games");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task GetGames_ReturnsListOfGames()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/games");
            var content = await response.Content.ReadAsStringAsync();
            var games = JsonConvert.DeserializeObject<List<GameDTO>>(content);

            // Assert
            Assert.NotNull(games);
        }
    }
}