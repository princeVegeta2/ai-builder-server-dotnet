using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIBuilderServerDotnet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class PageController : ControllerBase
    {
        private readonly IPageRepository _pageRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public PageController(IPageRepository pageRepository, IProjectRepository projectRepository, IMapper mapper, IJwtService jwtService)
        {
            _pageRepository = pageRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("add-page")]
        public async Task<IActionResult> AddPage([FromBody] AddPageDto addPageDto)
        {
            // Get JWT from the user and find their id using it
            var userId = _jwtService.GetUserIdFromToken(User);

            // Get a project name 
            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, addPageDto.ProjectName);

            if (project == null)
            {
                return BadRequest("The project does not exist or you do not have permission to add a page to this project.");
            }

            // Map the AddPageDto to Page and set ProjectId
            var page = _mapper.Map<Page>(addPageDto);
            page.ProjectId = project.Id;

            // Add page to the database
            await _pageRepository.AddPage(page);


            return Ok(new { message = "Page added successfully." });
        }

        [HttpDelete("delete-page")]
        public async Task<IActionResult> DeletePage([FromBody] DeletePageDto deletePageDto)
        {
            // Get JWT from the user and find their id using it
            var userId = _jwtService.GetUserIdFromToken(User);

            // Get a project name 
            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, deletePageDto.ProjectName);

            if (project == null)
            {
                return BadRequest("The project does not exist or you do not have permission to add a page to this project.");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, deletePageDto.Name);

            if (page == null)
            {
                return BadRequest("Failed to find the page to delete");
            }

            // Delete page from the database
            await _pageRepository.DeletePage(page);


            return Ok(new { message = "Page deleted successfully." });
        }

        [HttpPut("update-page")]
        public async Task<IActionResult> UpdatePage([FromBody] UpdatePageDto updatePageDto)
        {
            // Get JWT from the user and find their id using it
            var userId = _jwtService.GetUserIdFromToken(User);

            var project = await _projectRepository.GetProjectByUserIdAndProjectName(userId, updatePageDto.ProjectName);

            if (project == null)
            {
                return BadRequest("The project does not exist or you do not have permission to add a page to this project.");
            }

            var page = await _pageRepository.GetPageByProjectIdAndName(project.Id, updatePageDto.Name);

            if (page == null)
            {
                return BadRequest("Failed to find the page to update");
            }

            if (await _pageRepository.PageExistsForProjectAsync(project.Id, updatePageDto.NewName))
            {
                return BadRequest("A page with this name already exists in the project.");
            }

            // Manually update the fields
            page.Name = updatePageDto.NewName;

            // Update the page in the database
            await _pageRepository.UpdatePage(page);

            return Ok(new { message = "Page updated successfully." });
        }


    }
}
