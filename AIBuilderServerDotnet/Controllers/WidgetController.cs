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
    public class WidgetController : ControllerBase
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public WidgetController(IWidgetRepository widgetRepository, IProjectRepository projectRepository, 
            IPageRepository pageRepository, IMapper mapper, IJwtService jwtService)
        {
            _widgetRepository = widgetRepository;
            _projectRepository = projectRepository;
            _pageRepository = pageRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("add-widget")]
        public async Task<IActionResult> AddWidget([FromBody] AddWidgetDto addWidgetDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addWidgetDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, addWidgetDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = _mapper.Map<Widget>(addWidgetDto);
            widget.PageId = page.Id;

            // Adding a widget
            await _widgetRepository.AddWidget(widget);

            return Ok(new { message = "Widget added successfully." });
        }

        [HttpDelete("delete-widget")]
        public async Task<IActionResult> DeleteWidget([FromBody] DeleteWidgetDto deleteWidgetDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deleteWidgetDto.ProjectName);

            if (project == null)
            {
                return BadRequest("Project not found");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deleteWidgetDto.PageName);

            if (page == null)
            {
                return BadRequest("Page not found");
            }

            var widget = await _widgetRepository.GetWidgetByPageIdAndPosition(page.Id, deleteWidgetDto.Position);

            if (widget == null)
            {
                return BadRequest("Widget not found");
            }

            // Delete the widget
            await _widgetRepository.DeleteWidget(widget);
            // Update positions of higher widgets
            await _widgetRepository.UpdateWidgetPositionsForAPage(page.Id, deleteWidgetDto.Position);

            return Ok(new { message = "Widget deleted successfully." });
        }
    }
}
