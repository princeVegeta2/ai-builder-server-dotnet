using Xunit;
using Moq;
using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIBuilderServerDotnet.Tests
{
    public class UserDataControllerTests
    {
        private readonly UserDataController _userDataController;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        // private readonly string _jwtSecret;

        public UserDataControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            // _jwtSecret = "f0efbad8df5cfea3f90f1ede40099298b92db6ccd1ea7519c26338f289c00a98e9469b81f7f905d0f157a0b6dd1b4b82ababdd01550cc8ea5a3e1b8e2835e470";
            _userDataController = new UserDataController(_mockUserRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CheckBuilderAccess_ReturnsOk_WhenUserHasBuilderAccess()
        {
            // Arrange
            var userId = 1;
            var builderAccess = true;

            // Mock the JWT claims to return the userId
            _userDataController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _userDataController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

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

            // Mock the JWT claims to return the userId
            _userDataController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _userDataController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

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

            // Mock the JWT claims to return the userId
            _userDataController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _userDataController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

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
