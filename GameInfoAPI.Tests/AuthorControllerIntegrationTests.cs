using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GameInfoAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace GameInfoAPI.Tests.Integration
{
    public class AuthorControllerIntegrationTests : IClassFixture<WebApplicationFactory<GameInfoAPI.Program>>
    {
        private readonly WebApplicationFactory<GameInfoAPI.Program> _factory;

        public AuthorControllerIntegrationTests(WebApplicationFactory<GameInfoAPI.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateAuthor_AddsNewAuthor()
        {
            var client = _factory.CreateClient();
            var newAuthor = new AuthorDTO { Name = "New Author" };

            var content = new StringContent(JsonConvert.SerializeObject(newAuthor), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/authors", content);
            response.EnsureSuccessStatusCode();

            var createdAuthor = JsonConvert.DeserializeObject<AuthorDTO>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(createdAuthor);
            Assert.Equal("New Author", createdAuthor.Name);
        }

    }
}