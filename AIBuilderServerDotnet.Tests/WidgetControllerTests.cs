using Xunit;
using Moq;
using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using System.Security.Claims;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Tests
{
    public class WidgetControllerTests
    {
        private readonly WidgetController _widgetController;
        private readonly Mock<IWidgetRepository> _mockWidgetRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IPageRepository> _mockPageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;

        public WidgetControllerTests()
        {
            _mockWidgetRepository = new Mock<IWidgetRepository>();
            _mockPageRepository = new Mock<IPageRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockPageRepository = new Mock<IPageRepository>();
            _mockJwtService = new Mock<IJwtService>();
            _mockMapper = new Mock<IMapper>();
            _widgetController = new WidgetController(
                _mockWidgetRepository.Object,
                _mockProjectRepository.Object,
                _mockPageRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object
                );
        }

        [Fact]
        public async Task AddWidget_ReturnsOkResult_WhenWidgetIsAddedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var projectName = "Test project";
            var pageName = "Test page";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = 1 };

            var addWidgetDto = new AddWidgetDto
            {
                Type = "navbar",
                Position = 1,
                ProjectName = projectName,
                PageName = pageName
            };

            // Mock JWT service to return the correct user ID
            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            // Mock the repository methods to return the appropriate objects
            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync(project);

            _mockPageRepository.Setup(repo => repo.GetPageByProjectIdAndName(project.Id, pageName))
                               .ReturnsAsync(page);

            _mockMapper.Setup(m => m.Map<Widget>(addWidgetDto)).Returns(widget);

            // Act
            var result = await _widgetController.AddWidget(addWidgetDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockWidgetRepository.Verify(repo => repo.AddWidget(It.IsAny<Widget>()), Times.Once);
        }

        [Fact]
        public async Task DeleteWidget_ReturnsOkResult_WhenWidgetIsDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };

            var deleteWidgetDto = new DeleteWidgetDto
            {
                Position = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
            };

            // Mock JWT service to return the correct user ID
            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            // Mock the repository methods to return the appropriate objects
            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync(project);

            _mockPageRepository.Setup(repo => repo.GetPageByProjectIdAndName(project.Id, pageName))
                               .ReturnsAsync(page);

            _mockWidgetRepository.Setup(repo => repo.GetWidgetByPageIdAndPosition(page.Id, widgetPosition))
                                 .ReturnsAsync(widget);

            // Act
            var result = await _widgetController.DeleteWidget(deleteWidgetDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the widget was deleted
            _mockWidgetRepository.Verify(repo => repo.DeleteWidget(widget), Times.Once);

            // Verify that the widget positions were updated
            _mockWidgetRepository.Verify(repo => repo.UpdateWidgetPositionsForAPage(page.Id, widgetPosition), Times.Once);
        }

    }
}
