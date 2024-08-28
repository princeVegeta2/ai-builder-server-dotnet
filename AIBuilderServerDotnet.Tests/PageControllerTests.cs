using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace AIBuilderServerDotnet.Tests
{
    public class PageControllerTests
    {
        private readonly Mock<IPageRepository> _mockPageRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly PageController _pageController;

        public PageControllerTests()
        {
            _mockPageRepository = new Mock<IPageRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockJwtService = new Mock<IJwtService>();

            _pageController = new PageController(
                _mockPageRepository.Object,
                _mockProjectRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object // Pass the mock JwtService
            );
        }

        [Fact]
        public async Task AddPage_ReturnsOkResult_WhenPageIsAddedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var projectName = "Test Project";
            var addPageDto = new AddPageDto
            {
                Name = "Test Page",
                Position = 1,
                ProjectName = projectName
            };

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = addPageDto.Name, Position = addPageDto.Position, ProjectId = project.Id };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync(project);

            _mockMapper.Setup(m => m.Map<Page>(addPageDto)).Returns(page);

            // Act
            var result = await _pageController.AddPage(addPageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockPageRepository.Verify(repo => repo.AddPage(It.IsAny<Page>()), Times.Once);
        }

        [Fact]
        public async Task AddPage_ReturnsBadRequest_WhenProjectDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var projectName = "Nonexistent Project";
            var addPageDto = new AddPageDto
            {
                Name = "Test Page",
                Position = 1,
                ProjectName = projectName
            };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync((Project)null);

            // Act
            var result = await _pageController.AddPage(addPageDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            _mockPageRepository.Verify(repo => repo.AddPage(It.IsAny<Page>()), Times.Never);
        }

        [Fact]
        public async Task DeletePage_ReturnsOkResult_WhenPageIsDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var projectId = 1;
            var projectName = "Test Project";
            var pageName = "Test Page";
            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, Position = 1, ProjectId = project.Id };
            var deletePageDto = new DeletePageDto { ProjectName = projectName, Name = pageName };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync(project);

            _mockPageRepository.Setup(repo => repo.GetPageByProjectIdAndName(projectId, pageName))
                                   .ReturnsAsync(page);

            // Act
            var result = await _pageController.DeletePage(deletePageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockPageRepository.Verify(repo => repo.DeletePage(It.IsAny<Page>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePage_ReturnsOkResult_WhenPageIsUpdatedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var projectId = 1;
            var projectName = "Test Project";
            var pageName = "Test Page";
            var newPageName = "Updated Test Page";
            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, Position = 1, ProjectId = project.Id };
            var updatePageDto = new UpdatePageDto { ProjectName = projectName, Name = pageName, NewName = newPageName };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository.Setup(repo => repo.GetProjectByUserIdAndProjectName(userId, projectName))
                                  .ReturnsAsync(project);

            _mockPageRepository.Setup(repo => repo.GetPageByProjectIdAndName(projectId, pageName))
                                   .ReturnsAsync(page);

            // Act
            var result = await _pageController.UpdatePage(updatePageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockPageRepository.Verify(repo => repo.UpdatePage(It.IsAny<Page>()), Times.Once);
        }
    }
}
