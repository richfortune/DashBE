using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();

            // Creiamo un'istanza di UserService con i mock
            _userService = new UserService(_mockUserRepository.Object, null, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_UserDTO_If_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 1;

            var user = new User(userId, "testuser", "test@example.com", "hashedpassword");
            var role = new Role(roleId, "Admin");

            //assegniamo il ruolo all'utenza
            user.AssignRole(role);
            
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.Roles.Should().Contain("Admin");

            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_Should_Add_New_User()
        {
            // Arrange
            var username = "newuser";
            var email = "newuser@example.com";
            var password = "Password123";

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);

            _mockPasswordHasher.Setup(ph => ph.HashPassword(It.IsAny<User>(), password)).Returns("hashedpassword");

            _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await _userService.RegisterAsync(username, email, password);

            // Assert
            await act.Should().NotThrowAsync();
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_Should_Throw_Exception_If_Username_Exists()
        {
            // Arrange
            var existingUser = new User(Guid.NewGuid(), "existinguser", "existing@example.com", "hashedpassword");

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(existingUser.Username)).ReturnsAsync(existingUser);

            // Act
            Func<Task> act = async () => await _userService.RegisterAsync(existingUser.Username, "newemail@example.com", "Password123");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Username già in uso.");
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_User_If_Credentials_Are_Correct()
        {
            // Arrange
            var username = "validuser";
            var password = "Password123";
            var hashedPassword = "hashedpassword";
            var user = new User(Guid.NewGuid(), username, "valid@example.com", hashedPassword);

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, password)).Returns(PasswordVerificationResult.Success);

            // Act
            var result = await _userService.AuthenticateAsync(username, password);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_Null_If_Credentials_Are_Wrong()
        {
            // Arrange
            var username = "invaliduser";
            var password = "WrongPassword";
            var hashedPassword = "hashedpassword";
            var user = new User(Guid.NewGuid(), username, "invalid@example.com", hashedPassword);

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(ph => ph.VerifyHashedPassword(user, hashedPassword, password)).Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _userService.AuthenticateAsync(username, password);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AssignRoleAsync_Should_Throw_Exception_If_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = 1;
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _userService.AssignRoleAsync(userId, roleId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Utente non trovato");
        }

    }
}
