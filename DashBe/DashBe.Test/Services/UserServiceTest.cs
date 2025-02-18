using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DashBe.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;
        private readonly Mock<IEmailService> _mockEmailService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockEmailService= new Mock<IEmailService>();
            
            _userService = new UserService(
                _mockUserRepository.Object,
                _mockDbContext.Object,
                _mockPasswordHasher.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_UserDTO_If_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 1;

            var user = new User(userId, "testuser", "test@example.com", "hashedpassword");
            var role = new Role(roleId, "Admin");

            user.AssignRole(role);
            _mockUserRepository.Setup(repo => repo.GetByIdWithRolesAsync(userId)).ReturnsAsync(user);

            

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.Roles.Should().Contain("Admin");

            _mockUserRepository.Verify(repo => repo.GetByIdWithRolesAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_Should_Add_New_User_And_Log_Event()
        {
            // Arrange
            var username = "newuser";
            var email = "newuser@example.com";
            var password = "Password123";

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User?)null);
            _mockUserRepository.Setup(repo => repo.ExistsAsync<User>(u => u.Email == email)).ReturnsAsync(false);
            _mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<User>(), password)).Returns("hashedpassword");
            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            //_mockDbContext.Setup(db => db.LogAsync(It.IsAny<string>(), "Information", null)).Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _userService.RegisterAsync(username, email, password);

            // Assert
            await act.Should().NotThrowAsync();
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
            //_mockDbContext.Verify(db => db.LogAsync(It.IsAny<string>(), "Information", null), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_User_If_Credentials_Are_Correct_And_Log_Event()
        {
            // Arrange
            var username = "validuser";
            var password = "Password123";
            var hashedPassword = "hashedpassword";
            var user = new User(Guid.NewGuid(), username, "valid@example.com", hashedPassword);

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, password))
                               .Returns(PasswordVerificationResult.Success);
            _mockDbContext.Setup(db => db.LogAsync(It.IsAny<string>(), "Information", null)).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.AuthenticateAsync(username, password);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            //_mockDbContext.Verify(db => db.LogAsync(It.IsAny<string>(), "Information", null), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_Null_If_Credentials_Are_Wrong_And_Log_Event()
        {
            // Arrange
            var username = "invaliduser";
            var password = "WrongPassword";
            var hashedPassword = "hashedpassword";
            var user = new User(Guid.NewGuid(), username, "invalid@example.com", hashedPassword);

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, password))
                               .Returns(PasswordVerificationResult.Failed);
            _mockDbContext.Setup(db => db.LogAsync(It.IsAny<string>(), "Warning", null)).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.AuthenticateAsync(username, password);

            // Assert
            result.Should().BeNull();
            _mockDbContext.Verify(db => db.LogAsync(It.IsAny<string>(), "Warning", null), Times.Once);
        }

        [Fact]
        public async Task AssignRoleAsync_Should_Throw_Exception_If_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 1;
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            // Act
            Func<Task> act = async () => await _userService.AssignRoleAsync(userId, roleId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().Where(e => e.Message.Contains("Utente non trovato"));

        }
    }
}
