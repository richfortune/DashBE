using DashBe.Domain.Models;
using DashBe.Infrastructure.Services;
using DashBe.Application.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DashBe.Test.Repositories
{
    public class UserRepositoryTest
    {
        private Mock<IApplicationDbContext> _mockDbContext;
        private UserRepository _userRepository;

        public UserRepositoryTest()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();
            _userRepository = new UserRepository(_mockDbContext.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User_To_Database()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "testuser", "test@example.com", "hashedpassword");

            _mockDbContext.Setup(db => db.Insert(It.IsAny<User>()));
            _mockDbContext.Setup(db => db.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _userRepository.AddAsync(user);

            // Assert
            _mockDbContext.Verify(db => db.Insert(It.Is<User>(u => u.Id == user.Id)), Times.Once);
            _mockDbContext.Verify(db => db.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_If_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "testuser", "test@example.com", "hashedpassword");

            _mockDbContext.Setup(db => db.FirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>(), default))
                          .ReturnsAsync(user);

            // Act
            var result = await _userRepository.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_User_Does_Not_Exist()
        {
            // Arrange
            _mockDbContext.Setup(db => db.FirstOrDefaultAsync<User>(It.IsAny<Expression<Func<User, bool>>>(), default))
                          .ReturnsAsync((User?)null);

            // Act
            var result = await _userRepository.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AssignRoleAsync_Should_Add_RoleUser_To_Database()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 2;
            var roleUser = new RoleUser { UserId = userId, RoleId = roleId };

            _mockDbContext.Setup(db => db.Insert(It.IsAny<RoleUser>()));
            _mockDbContext.Setup(db => db.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _userRepository.AssignRoleAsync(userId, roleId);

            // Assert
            _mockDbContext.Verify(db => db.Insert(It.Is<RoleUser>(ru => ru.UserId == userId && ru.RoleId == roleId)), Times.Once);
            _mockDbContext.Verify(db => db.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UserRoleExistsAsync_Should_Return_True_If_Role_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 2;

            _mockDbContext.Setup(db => db.AnyAsync<RoleUser>(It.IsAny<Expression<Func<RoleUser, bool>>>(), default))
                          .ReturnsAsync(true);

            // Act
            var exists = await _userRepository.UserRoleExistsAsync(userId, roleId);

            // Assert
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task UserRoleExistsAsync_Should_Return_False_If_Role_Does_Not_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 2;

            _mockDbContext.Setup(db => db.AnyAsync<RoleUser>(It.IsAny<Expression<Func<RoleUser, bool>>>(), default))
                          .ReturnsAsync(false);

            // Act
            var exists = await _userRepository.UserRoleExistsAsync(userId, roleId);

            // Assert
            exists.Should().BeFalse();
        }
    }
}
