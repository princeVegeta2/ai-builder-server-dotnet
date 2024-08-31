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
    public class ModalValuesController : ControllerBase
    {
        private readonly IModalValuesRepository _modalValuesRepository;
        private readonly IModalRepository _modalRepository;
        private readonly IWidgetRepository _widgetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public ModalValuesController(IModalValuesRepository modalValuesRepository, IModalRepository modalRepository, 
            IWidgetRepository widgetRepository, IProjectRepository projectRepository, IPageRepository pageRepository, 
            IMapper mapper, IJwtService jwtService)
        {
            _modalValuesRepository = modalValuesRepository;
            _modalRepository = modalRepository;
            _widgetRepository = widgetRepository;
            _projectRepository = projectRepository;
            _pageRepository = pageRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("add-color")]
        public async Task<IActionResult> AddColorValue([FromBody] AddColorDto addColorDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addColorDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addColorDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, addColorDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }


            var colorModal = await _modalRepository.GetColorModalByWidgetIdAndPosition(widget.Id, addColorDto.WidgetPosition);

            if (colorModal == null)
            {
                return BadRequest("Modal not found");
            }


            if (await _modalValuesRepository.CheckIfColorValueExistsForAModal(addColorDto.Color, colorModal.Id))
            {
                return BadRequest("Color value already exists");
            }

            if (await _modalValuesRepository.PositionAlreadyExistsForColorModalId(addColorDto.Position, colorModal.Id))
            {
                await _modalValuesRepository.UpdateColorValue(addColorDto.Color, colorModal.Id, addColorDto.Position);
                return Ok(new { message = "Color updated successfully." });
            }


            var color = _mapper.Map<Color>(addColorDto);
            color.ColorModalId = colorModal.Id;
           
            await _modalValuesRepository.AddColorValue(color);

            return Ok(new { message = "Color added successfully." });
        }

        [HttpPost("add-link")]
        public async Task<IActionResult> AddLink([FromBody] AddLinkDto addLinkDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addLinkDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addLinkDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, addLinkDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var linkModal = await _modalRepository.GetLinkModalByWidgetIdAndPosition(widget.Id, addLinkDto.WidgetPosition);

            if (linkModal == null)
            {
                return BadRequest("Modal not found");
            }

            if (await _modalValuesRepository.CheckIfUrlExistsForAModal(addLinkDto.Url, linkModal.Id))
            {
                return BadRequest("Url already exists");
            }

            if (await _modalValuesRepository.PositionAlreadyExistsForLinkModalId(addLinkDto.Position, linkModal.Id))
            {
                await _modalValuesRepository.UpdateLinkUrl(addLinkDto.Url, linkModal.Id, addLinkDto.Position);
                return Ok(new { message = "Link updated successfully." });
            }

            var link = _mapper.Map<Link>(addLinkDto);
            link.LinkModalId = linkModal.Id;

            await _modalValuesRepository.AddLinkValue(link);
            return Ok(new { message = "Link added successfully." });
        }

        [HttpPost("add-image-link")]
        public async Task<IActionResult> AddImageLink([FromBody] AddImageLinkDto addImageLinkDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addImageLinkDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addImageLinkDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, addImageLinkDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var imageLinkModal = await _modalRepository.GetImageLinkModalByWidgetIdAndPosition(widget.Id, addImageLinkDto.WidgetPosition);

            if (imageLinkModal == null)
            {
                return BadRequest("Image link modal not found");
            }

            if (await _modalValuesRepository.CheckIfImageImageUrlExistsForAModal(addImageLinkDto.ImageUrl, imageLinkModal.Id))
            {
                return BadRequest("Image url already exists");
            }

            if (await _modalValuesRepository.PositionAlreadyExistsForImageLinkModalId(addImageLinkDto.Position, imageLinkModal.Id))
            {
                await _modalValuesRepository.UpdateImageLinkUrl(addImageLinkDto.ImageUrl, imageLinkModal.Id, addImageLinkDto.Position);
                return Ok(new { message = "Image link updated successfully." });
            }

            var imageLink = _mapper.Map<ImageLink>(addImageLinkDto);
            imageLink.ImageLinkModalId = imageLinkModal.Id;

            await _modalValuesRepository.AddImageLinkValue(imageLink);
            return Ok(new { message = "Image link added successfully." });
        }

        [HttpPost("add-prompt")]
        public async Task<IActionResult> AddPrompt([FromBody] AddPromptDto addPromptDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addPromptDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addPromptDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, addPromptDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var promptModal = await _modalRepository.GetPromptModalByWidgetIdAndPosition(widget.Id, addPromptDto.WidgetPosition);

            if (promptModal == null)
            {
                return BadRequest("Modal not found");
            }

            if (await _modalValuesRepository.CheckIfPromptExistsForAModal(addPromptDto.PromptValue, promptModal.Id))
            {
                return BadRequest("Prompt already exists");
            }

            if (await _modalValuesRepository.PositionAlreadyExistsForPrompModalId(addPromptDto.Position, promptModal.Id))
            {
                await _modalValuesRepository.UpdatePromptValue(addPromptDto.PromptValue, promptModal.Id, addPromptDto.Position);
                return Ok(new { message = "Prompt updated successfully." });
            }

            var prompt = _mapper.Map<Prompt>(addPromptDto);
            prompt.PromptModalId = promptModal.Id;

            await _modalValuesRepository.AddPromptValue(prompt);
            return Ok(new { message = "Image link added successfully." });
        }

        [HttpDelete("delete-color")]
        public async Task<IActionResult> DeleteColor([FromBody] DeleteValueDto deleteValueDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteValueDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteValueDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteValueDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var colorModal = await _modalRepository.GetColorModalByWidgetIdAndPosition(widget.Id, deleteValueDto.WidgetPosition);

            if (colorModal == null)
            {
                return BadRequest("Modal not found");
            }

            var color = await _modalValuesRepository.GetColorByColorModalIdAndPosition(colorModal.Id, deleteValueDto.Position);

            if (color == null)
            {
                return BadRequest("Color not found");
            }

            await _modalValuesRepository.DeleteColor(color);

            return Ok(new { message = "Color deleted successfully." });
        }

        [HttpDelete("delete-link")]
        public async Task<IActionResult> DeleteLink([FromBody] DeleteValueDto deleteValueDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteValueDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteValueDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteValueDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var linkModal = await _modalRepository.GetLinkModalByWidgetIdAndPosition(widget.Id, deleteValueDto.WidgetPosition);

            if (linkModal == null)
            {
                return BadRequest("Modal not found");
            }

            var link = await _modalValuesRepository.GetLinkByLinkModalIdAndPosition(linkModal.Id, deleteValueDto.Position);

            if (link == null)
            {
                return BadRequest("Link not found");
            }

            await _modalValuesRepository.DeleteLink(link);

            return Ok(new { message = "Link deleted successfully." });
        }

        [HttpDelete("delete-image-link")]
        public async Task<IActionResult> DeleteImageLink([FromBody] DeleteValueDto deleteValueDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteValueDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteValueDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteValueDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var imageLinkModal = await _modalRepository.GetImageLinkModalByWidgetIdAndPosition(widget.Id, deleteValueDto.WidgetPosition);

            if (imageLinkModal == null)
            {
                return BadRequest("Modal not found");
            }

            var imageLink = await _modalValuesRepository.GetImageLinkByImageLinkModalIdAndPosition(imageLinkModal.Id, deleteValueDto.Position);

            if (imageLinkModal == null)
            {
                return BadRequest("Image link not found");
            }

            await _modalValuesRepository.DeleteImageLink(imageLink);

            return Ok(new { message = "Image link deleted successfully." });
        }

        [HttpDelete("delete-prompt")]
        public async Task<IActionResult> DeletePrompt([FromBody] DeleteValueDto deleteValueDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteValueDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteValueDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteValueDto.WidgetPosition);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            var promptModal = await _modalRepository.GetPromptModalByWidgetIdAndPosition(widget.Id, deleteValueDto.WidgetPosition);

            if (promptModal == null)
            {
                return BadRequest("Modal not found");
            }

            var prompt = await _modalValuesRepository.GetPromptByPromptModalIdAndPosition(promptModal.Id, deleteValueDto.Position);

            if (prompt == null)
            {
                return BadRequest("Link not found");
            }

            await _modalValuesRepository.DeletePrompt(prompt);

            return Ok(new { message = "Link deleted successfully." });
        }




    }
}
