using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GameInfoAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GameInfoAPI.Tests.Integration
{
    public class PlayerControllerIntegrationTests : IClassFixture<WebApplicationFactory<GameInfoAPI.Program>>
    {
        private readonly WebApplicationFactory<GameInfoAPI.Program> _factory;

        public PlayerControllerIntegrationTests(WebApplicationFactory<GameInfoAPI.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreatePlayer_AddsNewPlayer()
        {
            var client = _factory.CreateClient();
            var newPlayer = new PlayerDTO { Name = "New Player" };

            var content = new StringContent(JsonConvert.SerializeObject(newPlayer), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/players", content);
            response.EnsureSuccessStatusCode();

            var createdPlayer = JsonConvert.DeserializeObject<PlayerDTO>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(createdPlayer);
            Assert.Equal("New Player", createdPlayer.Name);
        }

    }
}