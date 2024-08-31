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
using Microsoft.IdentityModel.Tokens;

namespace AIBuilderServerDotnet.Tests
{
    public class ModalValuesControllerTests
    {
        private readonly ModalValuesController _modalValuesController;
        private readonly Mock<IModalValuesRepository> _mockModalValuesRepository;
        private readonly Mock<IModalRepository> _mockModalRepository;
        private readonly Mock<IWidgetRepository> _mockWidgetRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IPageRepository> _mockPageRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJwtService> _mockJwtService;

        public ModalValuesControllerTests()
        {
            _mockModalValuesRepository = new Mock<IModalValuesRepository>();
            _mockModalRepository = new Mock<IModalRepository>();
            _mockWidgetRepository = new Mock<IWidgetRepository>();
            _mockWidgetRepository = new Mock<IWidgetRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockPageRepository = new Mock<IPageRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockJwtService = new Mock<IJwtService>();

            _modalValuesController = new ModalValuesController(
                _mockModalValuesRepository.Object,
                _mockModalRepository.Object,
                _mockWidgetRepository.Object,
                _mockProjectRepository.Object,
                _mockPageRepository.Object,
                _mockMapper.Object,
                _mockJwtService.Object
                );
        }

        [Fact]
        public async Task AddColorValue_ReturnsOkResult_WhenColorAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var colorModalPosition = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalType = "color";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var colorModal = new ColorModal { Id = 1, WidgetId = widget.Id, Position = colorModalPosition };
            var color = new Color { Id = 1, ColorModalId = colorModal.Id, Position = position, ColorValue = "#FF0000" };

            var addColorDto = new AddColorDto
            {
                Position = position,
                Color = "#FF0000",
                ProjectName = projectName,
                PageName = pageName,
                ModalType = modalType,
                WidgetPosition = widgetPosition,
                ModalPosition = colorModalPosition
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

            _mockModalRepository.Setup(repo => repo.GetColorModalByWidgetIdAndPosition(widget.Id, colorModalPosition))
                                .ReturnsAsync(colorModal);

            _mockMapper.Setup(m => m.Map<Color>(addColorDto)).Returns(color);

            // Act
            var result = await _modalValuesController.AddColorValue(addColorDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.AddColorValue(color), Times.Once);

        }

        [Fact]
        public async Task AddLink_ReturnsOkResult_WhenLinkAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var linkModalPosition = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalType = "link";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var linkModal = new LinkModal { Id = 1, WidgetId = widget.Id, Position = linkModalPosition };
            var link = new Link { Id = 1, LinkModalId = linkModal.Id, Position = position, Url = "https://www.example.com" };

            var addLinkDto = new AddLinkDto
            {
                Position = position,
                Url = "https://www.example.com",
                ProjectName = projectName,
                PageName = pageName,
                ModalType = modalType,
                WidgetPosition = widgetPosition,
                ModalPosition = linkModalPosition
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

            _mockModalRepository.Setup(repo => repo.GetLinkModalByWidgetIdAndPosition(widget.Id, linkModalPosition))
                                .ReturnsAsync(linkModal);

            _mockMapper.Setup(m => m.Map<Link>(addLinkDto)).Returns(link);

            // Act
            var result = await _modalValuesController.AddLink(addLinkDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.AddLinkValue(link), Times.Once);

        }

        [Fact]
        public async Task AddImageLink_ReturnsOkResult_WhenImageLinkAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var imageLinkModalPosition = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalType = "imageLink";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var imageLinkModal = new ImageLinkModal { Id = 1, WidgetId = widget.Id, Position = imageLinkModalPosition };
            var imageLink = new ImageLink { Id = 1, ImageLinkModalId = imageLinkModal.Id, Position = position, ImageUrl = "https://www.example.com/image.jpg" };

            var addImageLinkDto = new AddImageLinkDto
            {
                Position = position,
                ImageUrl = "https://www.example.com/image.jpg",
                ProjectName = projectName,
                PageName = pageName,
                ModalType = modalType,
                WidgetPosition = widgetPosition,
                ModalPosition = imageLinkModalPosition
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

            _mockModalRepository.Setup(repo => repo.GetImageLinkModalByWidgetIdAndPosition(widget.Id, imageLinkModalPosition))
                                .ReturnsAsync(imageLinkModal);

            _mockMapper.Setup(m => m.Map<ImageLink>(addImageLinkDto)).Returns(imageLink);

            // Act
            var result = await _modalValuesController.AddImageLink(addImageLinkDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.AddImageLinkValue(imageLink), Times.Once);
        }

        [Fact]
        public async Task AddPrompt_ReturnsOkResult_WhenPromptAddedSuccesfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var promptModalPosition = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalType = "prompt";

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var promptModal = new PromptModal { Id = 1, WidgetId = widget.Id, Position = promptModalPosition };
            var prompt = new Prompt { Id = 1, PromptModalId = promptModal.Id, Position = position, PromptValue = "Example" };

            var addPromptDto = new AddPromptDto
            {
                Position = position,
                PromptValue = "Example",
                ProjectName = projectName,
                PageName = pageName,
                ModalType = modalType,
                WidgetPosition = widgetPosition,
                ModalPosition = promptModalPosition
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

            _mockModalRepository.Setup(repo => repo.GetPromptModalByWidgetIdAndPosition(widget.Id, promptModalPosition))
                                .ReturnsAsync(promptModal);

            _mockMapper.Setup(m => m.Map<Prompt>(addPromptDto)).Returns(prompt);

            // Act
            var result = await _modalValuesController.AddPrompt(addPromptDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.AddPromptValue(prompt), Times.Once);

        }

        [Fact]
        public async Task DeleteColor_ReturnsOkResult_WhenColorDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalPosition = 1;

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var colorModal = new ColorModal { Id = 1, WidgetId = widget.Id, Position = modalPosition };
            var color = new Color { Id = 1, ColorModalId = colorModal.Id, Position = position };

            var deleteValueDto = new DeleteValueDto
            {
                Position = position,
                ProjectName = projectName,
                PageName = pageName,
                WidgetPosition = widgetPosition
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

            _mockModalRepository.Setup(repo => repo.GetColorModalByWidgetIdAndPosition(widget.Id, modalPosition))
                                .ReturnsAsync(colorModal);

            _mockModalValuesRepository.Setup(repo => repo.GetColorByColorModalIdAndPosition(colorModal.Id, position))
                                      .ReturnsAsync(color);

            // Act
            var result = await _modalValuesController.DeleteColor(deleteValueDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.DeleteColor(color), Times.Once);
        }

        [Fact]
        public async Task DeleteLink_ReturnsOkResult_WhenLinkDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalPosition = 1;

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var linkModal = new LinkModal { Id = 1, WidgetId = widget.Id, Position = modalPosition };
            var link = new Link { Id = 1, LinkModalId = linkModal.Id, Position = position };

            var deleteValueDto = new DeleteValueDto
            {
                Position = position,
                ProjectName = projectName,
                PageName = pageName,
                WidgetPosition = widgetPosition
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

            _mockModalRepository.Setup(repo => repo.GetLinkModalByWidgetIdAndPosition(widget.Id, modalPosition))
                                .ReturnsAsync(linkModal);

            _mockModalValuesRepository.Setup(repo => repo.GetLinkByLinkModalIdAndPosition(linkModal.Id, position))
                                      .ReturnsAsync(link);

            // Act
            var result = await _modalValuesController.DeleteLink(deleteValueDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.DeleteLink(link), Times.Once);
        }


        [Fact]
        public async Task DeleteImageLink_ReturnsOkResult_WhenImageLinkDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalPosition = 1;

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var imageLinkModal = new ImageLinkModal { Id = 1, WidgetId = widget.Id, Position = modalPosition };
            var imageLink = new ImageLink { Id = 1, ImageLinkModalId = imageLinkModal.Id, Position = position };

            var deleteValueDto = new DeleteValueDto
            {
                Position = position,
                ProjectName = projectName,
                PageName = pageName,
                WidgetPosition = widgetPosition
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

            _mockModalRepository.Setup(repo => repo.GetImageLinkModalByWidgetIdAndPosition(widget.Id, modalPosition))
                                .ReturnsAsync(imageLinkModal);

            _mockModalValuesRepository.Setup(repo => repo.GetImageLinkByImageLinkModalIdAndPosition(imageLinkModal.Id, position))
                                      .ReturnsAsync(imageLink);

            // Act
            var result = await _modalValuesController.DeleteImageLink(deleteValueDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.DeleteImageLink(imageLink), Times.Once);
        }

        [Fact]
        public async Task DeletePrompt_ReturnsOkResult_WhenPromptDeletedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var position = 1;
            var projectName = "Test project";
            var pageName = "Test page";
            var widgetPosition = 1;
            var modalPosition = 1;

            var project = new Project { Id = 1, Name = projectName, UserId = userId };
            var page = new Page { Id = 1, Name = pageName, ProjectId = project.Id };
            var widget = new Widget { Id = 1, PageId = page.Id, Type = "navbar", Position = widgetPosition };
            var promptModal = new PromptModal { Id = 1, WidgetId = widget.Id, Position = modalPosition };
            var prompt = new Prompt { Id = 1, PromptModalId = promptModal.Id, Position = position, PromptValue = "Example Prompt" };

            var deleteValueDto = new DeleteValueDto
            {
                Position = position,
                ProjectName = projectName,
                PageName = pageName,
                WidgetPosition = widgetPosition
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

            _mockModalRepository.Setup(repo => repo.GetPromptModalByWidgetIdAndPosition(widget.Id, modalPosition))
                                .ReturnsAsync(promptModal);

            _mockModalValuesRepository.Setup(repo => repo.GetPromptByPromptModalIdAndPosition(promptModal.Id, position))
                                      .ReturnsAsync(prompt);

            // Act
            var result = await _modalValuesController.DeletePrompt(deleteValueDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            _mockModalValuesRepository.Verify(repo => repo.DeletePrompt(prompt), Times.Once);
        }



    }
}
