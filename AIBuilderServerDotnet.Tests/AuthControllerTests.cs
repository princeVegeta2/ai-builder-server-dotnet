using Xunit;
using Moq;
using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;

namespace AIBuilderServerDotnet.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly string _jwtSecret;

        public AuthControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _jwtSecret = "f0efbad8df5cfea3f90f1ede40099298b92db6ccd1ea7519c26338f289c00a98e9469b81f7f905d0f157a0b6dd1b4b82ababdd01550cc8ea5a3e1b8e2835e470"; // Set your test JWT secret here
            _authController = new AuthController(_mockUserRepository.Object, _mockMapper.Object, _jwtSecret);
        }

        [Fact]
        public async Task SignUp_ReturnsOk_WhenUserIsRegistered()
        {
            // Arrange
            var signUpDto = new SignUpDto
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            // Mocking mapper
            var user = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.UserExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockUserRepository.Setup(repo => repo.UserExistsByUsernameAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Mock mapper behavior of AutoMapper
            _mockMapper.Setup(m => m.Map<User>(It.IsAny<SignUpDto>())).Returns(user);

            // Act
            var result = await _authController.SignUp(signUpDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully.", okResult.Value);
        }

        [Fact]
        public async Task SignIn_ReturnsOk_WithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var signInDto = new SignInDto
            {
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var user = new User
            {
                Id = 1,
                Email = "testuser@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!")
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await _authController.SignIn(signInDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic tokenResponse = okResult.Value;
            Assert.NotNull(tokenResponse.Token);
        }
    }
}
