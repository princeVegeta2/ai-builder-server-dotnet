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
        private readonly Mock<IJwtService> _mockJwtService;

        public AuthControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockJwtService = new Mock<IJwtService>();

            _authController = new AuthController(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object // Inject the mocked JwtService
            );
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

            var user = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _mockUserRepository.Setup(repo => repo.UserExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockUserRepository.Setup(repo => repo.UserExistsByUsernameAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockMapper.Setup(m => m.Map<User>(It.IsAny<SignUpDto>())).Returns(user);

            // Act
            var result = await _authController.SignUp(signUpDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
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

            var expectedToken = "mocked_jwt_token";

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockJwtService.Setup(service => service.GenerateToken(user.Id, user.Email, It.IsAny<bool>()))
                .Returns(expectedToken);

            // Act
            var result = await _authController.SignIn(signInDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResponse = okResult.Value as SignInResponseDto;

            Assert.NotNull(tokenResponse);
            Assert.Equal(expectedToken, tokenResponse.Token);
        }
    }
}
