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

        [Fact]
        public async Task GetUserProjectNames_ReturnsOkResult_WithProjectNames()
        {
            // Arrange
            var userId = 1;
            var projects = new List<Project>
    {
        new Project { Id = 1, Name = "Project 1", UserId = userId },
        new Project { Id = 2, Name = "Project 2", UserId = userId },
        new Project { Id = 3, Name = "Project 3", UserId = userId }
    };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockProjectRepository.Setup(repo => repo.GetProjectsByUserId(userId))
                .ReturnsAsync(projects);

            // Act
            var result = await _projectController.GetUserProjectNames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var projectNames = Assert.IsType<List<string>>(okResult.Value);

            Assert.Equal(3, projectNames.Count);
            Assert.Contains("Project 1", projectNames);
            Assert.Contains("Project 2", projectNames);
            Assert.Contains("Project 3", projectNames);

            _mockProjectRepository.Verify(repo => repo.GetProjectsByUserId(userId), Times.Once);
        }

        [Fact]
        public async Task GetProjectDetails_ReturnsOkResult_WithProjectDetails()
        {
            // Arrange
            var userId = 1;
            var projectName = "Test Project";

            // Mock the ProjectDto response for the method
            var projectDto = new ProjectDto
            {
                ProjectName = projectName,
                Pages = new List<PageDto>
        {
            new PageDto
            {
                PageName = "Page 1",
                Position = 1,
                Widgets = new List<WidgetDto>
                {
                    new WidgetDto
                    {
                        Type = "WidgetType1",
                        Position = 1,
                        ColorModal = new ColorModalDto
                        {
                            Position = 1,
                            Colors = new List<ColorValueDto>
                            {
                                new ColorValueDto { Color = "#FF0000", Position = 1 }
                            }
                        },
                        LinkModal = new LinkModalDto
                        {
                            Position = 1,
                            Links = new List<LinkValueDto>
                            {
                                new LinkValueDto { Name = "Link1", Url = "http://example.com", Position = 1 }
                            }
                        },
                        ImageLinkModal = new ImageLinkModalDto
                        {
                            Position = 1,
                            ImageLinks = new List<ImageLinkValueDto>
                            {
                                new ImageLinkValueDto { Url = "http://example.com/image.png", Position = 1 }
                            }
                        },
                        PromptModal = new PromptModalDto
                        {
                            Position = 1,
                            Prompt = "Sample prompt"
                        }
                    }
                }
            }
        }
            };

            // Mock JWT service to return the correct user ID
            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            // Mock the repository to return the mocked ProjectDto
            _mockProjectRepository.Setup(repo => repo.GetProjectDetails(userId, projectName))
                .ReturnsAsync(projectDto);

            // Act
            var result = await _projectController.GetProjectDetails(projectName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProject = Assert.IsType<ProjectDto>(okResult.Value);

            Assert.Equal(projectName, returnedProject.ProjectName);
            Assert.Single(returnedProject.Pages);
            Assert.Equal("Page 1", returnedProject.Pages[0].PageName);
            Assert.Single(returnedProject.Pages[0].Widgets);
            Assert.Equal("WidgetType1", returnedProject.Pages[0].Widgets[0].Type);
            Assert.Equal("#FF0000", returnedProject.Pages[0].Widgets[0].ColorModal.Colors[0].Color);
            Assert.Equal("Sample prompt", returnedProject.Pages[0].Widgets[0].PromptModal.Prompt);
            _mockProjectRepository.Verify(repo => repo.GetProjectDetails(userId, projectName), Times.Once);
        }


        [Fact]
        public async Task DeleteProject_ProjectExists_ReturnsOk()
        {
            // Arrange
            var deleteProjectDto = new DeleteProjectDto { Name = "TestProject" };
            var userId = 1;
            var project = new Project { Name = deleteProjectDto.Name, UserId = userId };

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
            .Returns(userId);
            _mockProjectRepository.Setup(r => r.GetProjectByUserIdAndProjectName(userId, deleteProjectDto.Name))
                .ReturnsAsync(project);
            _mockProjectRepository.Setup(r => r.DeleteProject(project))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _projectController.DeleteProject(deleteProjectDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Project delete succesfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteProject_ProjectDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var deleteProjectDto = new DeleteProjectDto { Name = "NonExistentProject" };
            var userId = 1;

            _mockJwtService.Setup(service => service.GetUserIdFromToken(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
            _mockProjectRepository.Setup(r => r.GetProjectByUserIdAndProjectName(userId, deleteProjectDto.Name)).
                ReturnsAsync((Project)null);

            // Act
            var result = await _projectController.DeleteProject(deleteProjectDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
