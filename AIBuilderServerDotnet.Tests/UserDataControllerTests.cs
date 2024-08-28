using Xunit;
using Moq;
using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using System.Security.Claims;

namespace AIBuilderServerDotnet.Tests
{
    public class UserDataControllerTests
    {
        private readonly UserDataController _userDataController;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;

        public UserDataControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockJwtService = new Mock<IJwtService>();

            _userDataController = new UserDataController(_mockUserRepository.Object, _mockMapper.Object, _mockJwtService.Object);
        }

        [Fact]
        public async Task CheckBuilderAccess_ReturnsOk_WhenUserHasBuilderAccess()
        {
            // Arrange
            var userId = 1;
            var builderAccess = true;

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockUserRepository.Setup(repo => repo.UserHasBuilderAccessAsync(userId))
                .ReturnsAsync(builderAccess);

            // Act
            var result = await _userDataController.CheckBuilderAccess();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            Assert.NotNull(response);
            Assert.True((bool)response.GetType().GetProperty("BuilderAccess")?.GetValue(response, null));
        }

        [Fact]
        public async Task GetUsername_ReturnsOk_WhenUsernameExists()
        {
            // Arrange
            var userId = 1;
            var username = "testuser";

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockUserRepository.Setup(repo => repo.GetUsernameById(userId))
                .ReturnsAsync(username);

            // Act
            var result = await _userDataController.GetUsername();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            Assert.NotNull(response);
            Assert.Equal(username, response.GetType().GetProperty("Username")?.GetValue(response, null));
        }

        [Fact]
        public async Task GetEmail_ReturnsOk_WhenEmailExists()
        {
            // Arrange
            var userId = 1;
            var email = "testuser@example.com";

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockUserRepository.Setup(repo => repo.GetEmailById(userId))
                .ReturnsAsync(email);

            // Act
            var result = await _userDataController.GetEmail();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            Assert.NotNull(response);
            Assert.Equal(email, response.GetType().GetProperty("Email")?.GetValue(response, null));
        }
    }
}
