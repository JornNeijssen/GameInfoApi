using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameInfoAPI.DTOs;
using GameInfoAPI.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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
        public async Task CreateGame_AddsNewGame()
        {
            var client = _factory.CreateClient();


            var newGame = new GameDTO
            {
                Title = "New Game",
                Description = "New Description",
                ReleaseDate = DateTime.Now,
                AuthorId = 3,
                BestPlayerId = 3,
                Author = new AuthorDTO
                {
                    Id = 3, 
                    Name = ""
                },
                BestPlayer = new PlayerDTO
                {
                    Id = 3, 
                    Name = ""
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(newGame), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/games", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to create game. Status code: {response.StatusCode}. Response content: {responseContent}");
            }

            var createdGame = JsonConvert.DeserializeObject<GameDTO>(responseContent);
            Assert.NotNull(createdGame);
            Assert.Equal("New Game", createdGame.Title);
            Assert.Equal("New Description", createdGame.Description);
            Assert.NotEqual(0, createdGame.Id); 
            Assert.Equal(3, createdGame.Author.Id);
            Assert.Equal(3, createdGame.BestPlayer.Id);
        }
    }
}