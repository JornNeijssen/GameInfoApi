using System;
using System.Threading.Tasks;
using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameInfoAPI.Tests
{
    public class PlayerRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsPlayer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            using (var context = new DataContext(options))
            {
                var repository = new PlayerRepository(context);
                var player = new Player { Id = 1, Name = "Test Player" };
                await repository.CreateAsync(player);
            }

            using (var context = new DataContext(options))
            {
                // Act
                var repository = new PlayerRepository(context);
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Player", result.Name);
            }
        }
    }
}