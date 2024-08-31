using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AIBuilderServerDotnet.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AIBuilderServerDotnet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ModalController : ControllerBase
    {
        private readonly IModalRepository _modalRepository;
        private readonly IWidgetRepository _widgetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public ModalController(IModalRepository modalRepository, IWidgetRepository widgetRepository, 
            IProjectRepository projectRepository, IPageRepository pageRepository, IMapper mapper, IJwtService jwtService)
        {
            _modalRepository = modalRepository;
            _widgetRepository = widgetRepository;
            _projectRepository = projectRepository;
            _pageRepository = pageRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("add-modal")]
        public async Task<IActionResult> AddModal([FromBody] AddModalDto addModalDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addModalDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addModalDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, addModalDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            switch (addModalDto.Type)
            {
                case "color":
                    var colorModal = _mapper.Map<ColorModal>(addModalDto);
                    colorModal.WidgetId = widget.Id;
                    await _modalRepository.AddColorModal(colorModal);
                    break;

                case "link":
                    var linkModal = _mapper.Map<LinkModal>(addModalDto);
                    linkModal.WidgetId = widget.Id;
                    await _modalRepository.AddLinkModal(linkModal);
                    break;

                case "image-link":
                    var imageLinkModal = _mapper.Map<ImageLinkModal>(addModalDto);
                    imageLinkModal.WidgetId = widget.Id;
                    await _modalRepository.AddImageLinkModal(imageLinkModal);
                    break;

                case "prompt":
                    var promptModal = _mapper.Map<PromptModal>(addModalDto);
                    promptModal.WidgetId = widget.Id;
                    await _modalRepository.AddPromptModal(promptModal);
                    break;

                default:
                    return BadRequest("Invalid modal type");
            }

            return Ok(new { message = "Modal added successfully." });
        }

        [HttpDelete("delete-modal")]
        public async Task<IActionResult> DeleteModal([FromBody] DeleteModalDto deleteModalDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteModalDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteModalDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteModalDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            switch (deleteModalDto.Type)
            {
                case "color":
                    var colorModal = await _modalRepository.GetColorModalByWidgetIdAndPosition(widget.Id, deleteModalDto.Position);
                    await _modalRepository.DeleteColorModal(colorModal);
                    await _modalRepository.UpdateModalPositionsGlobal(widget.Id, deleteModalDto.Position);
                    break;
                case "link":
                    var linkModal = await _modalRepository.GetLinkModalByWidgetIdAndPosition(widget.Id, deleteModalDto.Position);
                    await _modalRepository.DeleteLinkModal(linkModal);
                    await _modalRepository.UpdateModalPositionsGlobal(widget.Id, deleteModalDto.Position);
                    break;
                case "image-link":
                    var imageLinkModal = await _modalRepository.GetImageLinkModalByWidgetIdAndPosition(widget.Id, deleteModalDto.Position);
                    await _modalRepository.DeleteImageLinkModal(imageLinkModal);
                    await _modalRepository.UpdateModalPositionsGlobal(widget.Id, deleteModalDto.Position);
                    break;
                case "prompt":
                    var promptModal = await _modalRepository.GetPromptModalByWidgetIdAndPosition(widget.Id, deleteModalDto.Position);
                    await _modalRepository.DeletePromptModal(promptModal);
                    await _modalRepository.UpdateModalPositionsGlobal(widget.Id, deleteModalDto.Position);
                    break;
                default:
                    return BadRequest("Invalid modal type");
            }

            return Ok(new { message = "Modal deleted successfully." });
        }

        

    }
}
