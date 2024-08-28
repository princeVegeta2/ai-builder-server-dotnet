using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace AIBuilderServerDotnet.Tests
{
    public class ProjectControllerTests
    {
        private readonly ProjectController _projectController;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;

        public ProjectControllerTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockJwtService = new Mock<IJwtService>(); // Initialize JwtService mock

            _projectController = new ProjectController(
                _mockProjectRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object
            );
        }

        [Fact]
        public async Task CreateProject_ReturnsOkResult_WhenProjectIsCreatedSuccessfully()
        {
            // Arrange
            var addProjectDto = new AddProjectDto { Name = "Test Project" };
            var userId = 1;

            var project = new Project { Id = 123, Name = addProjectDto.Name, UserId = userId };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockMapper.Setup(m => m.Map<Project>(It.IsAny<AddProjectDto>())).Returns(project);

            // Act
            var result = await _projectController.CreateProject(addProjectDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseValue = Assert.IsType<CreateProjectResponseDto>(okResult.Value);

            Assert.Equal("Project created successfully", responseValue.Message);
            Assert.Equal(123, responseValue.ProjectId);

            _mockProjectRepository.Verify(repo => repo.AddProject(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task CreateProject_ReturnsBadRequest_WhenProjectAlreadyExists()
        {
            // Arrange
            var addProjectDto = new AddProjectDto { Name = "Existing Project" };
            var userId = 1;

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository
                .Setup(repo => repo.ProjectExistsForUserAsync(userId, addProjectDto.Name))
                .ReturnsAsync(true); // Simulate project exists

            // Act
            var result = await _projectController.CreateProject(addProjectDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            _mockProjectRepository.Verify(repo => repo.AddProject(It.IsAny<Project>()), Times.Never);
            _mockProjectRepository.Verify(repo => repo.ProjectExistsForUserAsync(userId, addProjectDto.Name), Times.Once);
        }
    }
}
