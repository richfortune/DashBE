using DashBe.Domain.Models;
using DashBe.Infrastructure.Data;
using DashBe.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Test.Repositories
{
    public class UserRepositoryTest
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // In-Memory DB
                .Options;

            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        [Fact]
        public async Task AddAsync_Should_Add_User_To_Database()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);
            var user = new User(Guid.NewGuid(), "testuser", "test@example.com", "hashedpassword");

            // Act
            await userRepository.AddAsync(user);
            var result = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_If_Exists()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);
            var user = new User(Guid.NewGuid(), "testuser", "test@example.com", "hashedpassword");
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await userRepository.GetByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_User_Does_Not_Exist()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = await userRepository.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

      
    }
}
