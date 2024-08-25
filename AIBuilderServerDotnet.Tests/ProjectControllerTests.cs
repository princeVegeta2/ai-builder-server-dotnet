﻿using AIBuilderServerDotnet.Controllers;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace AIBuilderServerDotnet.Tests
{
    public class ProjectControllerTests
    {
        private readonly ProjectController _projectController;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;

        public ProjectControllerTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            _projectController = new ProjectController(
                _mockProjectRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task CreateProject_ReturnsOkResult_WhenProjectIsCreatedSuccessfully()
        {
            // Arrange
            var addProjectDto = new AddProjectDto { Name = "Test Project" };
            var userId = 1;

            // Mock the mapping from DTO to Project model
            var project = new Project { Name = addProjectDto.Name, UserId = userId };
            _mockMapper.Setup(m => m.Map<Project>(It.IsAny<AddProjectDto>())).Returns(project);

            // Mock the HttpContext and User Claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _projectController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _projectController.CreateProject(addProjectDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Project created successfully.", okResult.Value);

            /*
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully.", okResult.Value);
            */

            _mockProjectRepository.Verify(repo => repo.AddProject(It.IsAny<Project>()), Times.Once);
        }
    }
}