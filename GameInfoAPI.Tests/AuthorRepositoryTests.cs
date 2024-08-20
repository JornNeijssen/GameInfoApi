using GameInfoAPI.Data;
using GameInfoAPI.Entities;
using GameInfoAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GameInfoAPI.Tests
{
    public class AuthorRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(arrangeContext);
                var author = new Author { Id = 1, Name = "Test Author" };
                await repository.CreateAsync(author);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test Author", result.Name);
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAuthors()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(arrangeContext);
                await repository.CreateAsync(new Author { Id = 1, Name = "Author 1" });
                await repository.CreateAsync(new Author { Id = 2, Name = "Author 2" });
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var authors = await repository.GetAllAsync();

                // Assert
                Assert.Equal(2, authors.Count);
                Assert.Contains(authors, a => a.Name == "Author 1");
                Assert.Contains(authors, a => a.Name == "Author 2");
            }
        }

        [Fact]
        public async Task CreateAsync_AddsNewAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var author = new Author { Id = 1, Name = "New Author" };

                await repository.CreateAsync(author);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var result = await assertContext.Authors.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("New Author", result.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(arrangeContext);
                var author = new Author { Id = 1, Name = "AuthorToUpdate" };
                await repository.CreateAsync(author);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var authorToUpdate = await repository.GetByIdAsync(1);
                authorToUpdate.Name = "Updated Author";
                await repository.UpdateAsync(authorToUpdate);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var updatedAuthor = await assertContext.Authors.FindAsync(1);
                Assert.Equal("Updated Author", updatedAuthor.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(arrangeContext);
                var author = new Author { Id = 1, Name = "AuthorToDelete" };
                await repository.CreateAsync(author);
            }

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var authorToDelete = await repository.GetByIdAsync(1);
                await repository.DeleteAsync(authorToDelete);
            }

            // Assert
            using (var assertContext = new DataContext(dbContextOptions))
            {
                var deletedAuthor = await assertContext.Authors.FindAsync(1);
                Assert.Null(deletedAuthor);
            }
        }

        [Fact]
        public async Task GetOrCreateAsync_ReturnsExistingAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var arrangeContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(arrangeContext);
                var existingAuthor = new Author { Id = 1, Name = "Existing Author" };
                await repository.CreateAsync(existingAuthor);
            }

            // Act & Assert
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var result = await repository.GetOrCreateAsync(1, "Existing Author");

                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Existing Author", result.Name);
            }
        }

        [Fact]
        public async Task GetOrCreateAsync_CreatesNewAuthor()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var actContext = new DataContext(dbContextOptions))
            {
                var repository = new AuthorRepository(actContext);
                var result = await repository.GetOrCreateAsync(1, "New Author");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("New Author", result.Name);
            }
        }
    }
}