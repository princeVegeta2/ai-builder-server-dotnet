﻿using Xunit;
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
    public class ModalControllerTests
    {
        private readonly ModalController _modalController;
        private readonly Mock<IModalRepository> _mockModalRepository;
        private readonly Mock<IWidgetRepository> _mockWidgetRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IPageRepository> _mockPageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;

        public ModalControllerTests()
        {
            _mockModalRepository = new Mock<IModalRepository>();
            _mockWidgetRepository = new Mock<IWidgetRepository>();
            _mockPageRepository = new Mock<IPageRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockPageRepository = new Mock<IPageRepository>();
            _mockJwtService = new Mock<IJwtService>();
            _mockMapper = new Mock<IMapper>();
            _modalController = new ModalController(
                _mockModalRepository.Object,
                _mockWidgetRepository.Object,
                _mockProjectRepository.Object,
                _mockPageRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object
                );
        }

        [Fact]
        public async Task AddModal_ReturnsOkResult_WhenColorModalAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "color";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var colorModal = new ColorModal { Id = 1, WidgetId = widget.Id, Position = position};

            var addModalDto = new AddModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockMapper.Setup(m => m.Map<ColorModal>(addModalDto)).Returns(colorModal);

            // Act
            var result = await _modalController.AddModal(addModalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalRepository.Verify(repo => repo.AddColorModal(colorModal), Times.Once);


        }

        [Fact]
        public async Task AddModal_ReturnsOkResult_WhenLinkModalAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "link";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var linkModal = new LinkModal { Id = 1, WidgetId = widget.Id, Position = position };

            var addModalDto = new AddModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockMapper.Setup(m => m.Map<LinkModal>(addModalDto)).Returns(linkModal);

            // Act
            var result = await _modalController.AddModal(addModalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalRepository.Verify(repo => repo.AddLinkModal(linkModal), Times.Once);

        }

        [Fact]
        public async Task AddModal_ReturnsOkResult_WhenImageLinkModalAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "image-link";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var imageLinkModal = new ImageLinkModal { Id = 1, WidgetId = widget.Id, Position = position };

            var addModalDto = new AddModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockMapper.Setup(m => m.Map<ImageLinkModal>(addModalDto)).Returns(imageLinkModal);

            // Act
            var result = await _modalController.AddModal(addModalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalRepository.Verify(repo => repo.AddImageLinkModal(imageLinkModal), Times.Once);

        }

        [Fact]
        public async Task AddModal_ReturnsOkResult_WhenPromptModalAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "prompt";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var promptModal = new PromptModal { Id = 1, WidgetId = widget.Id, Position = position };

            var addModalDto = new AddModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockMapper.Setup(m => m.Map<PromptModal>(addModalDto)).Returns(promptModal);

            // Act
            var result = await _modalController.AddModal(addModalDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalRepository.Verify(repo => repo.AddPromptModal(promptModal), Times.Once);

        }

        [Fact]
        public async Task DeleteModal_ReturnsOkResult_WhenColorModalIsDeleted()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "color";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var colorModal = new ColorModal { Id = 1, WidgetId = widget.Id, Position = position };

            var deleteModalDto = new DeleteModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockModalRepository.Setup(repo => repo.GetColorModalByWidgetIdAndPosition(widget.Id, position))
                                .ReturnsAsync(colorModal);

            var result = await _modalController.DeleteModal(deleteModalDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the color modal was deleted
            _mockModalRepository.Verify(repo => repo.DeleteColorModal(colorModal), Times.Once);

            // Verify that the color modal positions were updated
            _mockModalRepository.Verify(repo => repo.UpdateModalPositionsGlobal(widget.Id, position), Times.Once);
        }

        [Fact]
        public async Task DeleteModal_ReturnsOkResult_WhenLinkModalIsDeleted()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "link";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var linkModal = new LinkModal { Id = 1, WidgetId = widget.Id, Position = position };

            var deleteModalDto = new DeleteModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockModalRepository.Setup(repo => repo.GetLinkModalByWidgetIdAndPosition(widget.Id, position))
                                .ReturnsAsync(linkModal);

            var result = await _modalController.DeleteModal(deleteModalDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the color modal was deleted
            _mockModalRepository.Verify(repo => repo.DeleteLinkModal(linkModal), Times.Once);

            // Verify that the color modal positions were updated
            _mockModalRepository.Verify(repo => repo.UpdateModalPositionsGlobal(widget.Id, position), Times.Once);
        }

        [Fact]
        public async Task DeleteModal_ReturnsOkResult_WhenImageLinkModalIsDeleted()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "image-link";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var imageLinkModal = new ImageLinkModal { Id = 1, WidgetId = widget.Id, Position = position };

            var deleteModalDto = new DeleteModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockModalRepository.Setup(repo => repo.GetImageLinkModalByWidgetIdAndPosition(widget.Id, position))
                                .ReturnsAsync(imageLinkModal);

            var result = await _modalController.DeleteModal(deleteModalDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the color modal was deleted
            _mockModalRepository.Verify(repo => repo.DeleteImageLinkModal(imageLinkModal), Times.Once);

            // Verify that the color modal positions were updated
            _mockModalRepository.Verify(repo => repo.UpdateModalPositionsGlobal(widget.Id, position), Times.Once);
        }

        [Fact]
        public async Task DeleteModal_ReturnsOkResult_WhenPromptModalIsDeleted()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var type = "prompt";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var promptModal = new PromptModal { Id = 1, WidgetId = widget.Id, Position = position };

            var deleteModalDto = new DeleteModalDto
            {
                Position = position,
                WidgetPosition = widgetPosition,
                ProjectName = projectName,
                PageName = pageName,
                Type = type
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

            _mockModalRepository.Setup(repo => repo.GetPromptModalByWidgetIdAndPosition(widget.Id, position))
                                .ReturnsAsync(promptModal);

            var result = await _modalController.DeleteModal(deleteModalDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the color modal was deleted
            _mockModalRepository.Verify(repo => repo.DeletePromptModal(promptModal), Times.Once);

            // Verify that the color modal positions were updated
            _mockModalRepository.Verify(repo => repo.UpdateModalPositionsGlobal(widget.Id, position), Times.Once);
        }
    }
}
