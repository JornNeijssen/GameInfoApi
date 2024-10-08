﻿using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GameInfoAPI.Tests
{
    public class PlayerRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(arrangeContext);
                var player = new Player { Id = 1, Name = "Test Player"};
                await repository.CreateAsync(player);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Player", result.Name);
                
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPlayers()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(arrangeContext);
                await repository.CreateAsync(new Player { Id = 1, Name = "Player 1"});
                await repository.CreateAsync(new Player { Id = 2, Name = "Player 2"});
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var players = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, players.Count);
                Assert.Contains(players, p => p.Name == "Player 1");
                Assert.Contains(players, p => p.Name == "Player 2");
            }
        }

        [Fact]
        public async Task CreateAsync_AddsNewPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var player = new Player { Id = 1, Name = "New Player"};

                await repository.CreateAsync(player);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var result = await assertContext.Players.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("New Player", result.Name);
                
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(arrangeContext);
                var player = new Player { Id = 1, Name = "PlayerToUpdate"};
                await repository.CreateAsync(player);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var playerToUpdate = await repository.GetByIdAsync(1);
                playerToUpdate.Name = "Updated Player";
                await repository.UpdateAsync(playerToUpdate);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var updatedPlayer = await assertContext.Players.FindAsync(1);
                Assert.Equal("Updated Player", updatedPlayer.Name);
                
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(arrangeContext);
                var player = new Player { Id = 1, Name = "PlayerToDelete"};
                await repository.CreateAsync(player);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var playerToDelete = await repository.GetByIdAsync(1);
                await repository.DeleteAsync(playerToDelete);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var deletedPlayer = await assertContext.Players.FindAsync(1);
                Assert.Null(deletedPlayer);
            }
        }

        [Fact]
        public async Task GetOrCreateAsync_ReturnsExistingPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(arrangeContext);
                var existingPlayer = new Player { Id = 1, Name = "Existing Player"};
                await repository.CreateAsync(existingPlayer);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var result = await repository.GetOrCreateAsync(1, "Existing Player");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Existing Player", result.Name);
                
            }
        }

        [Fact]
        public async Task GetOrCreateAsync_CreatesNewPlayer()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new PlayerRepository(actContext);
                var result = await repository.GetOrCreateAsync(1, "New Player");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("New Player", result.Name); 
                
            }
        }
    }
}