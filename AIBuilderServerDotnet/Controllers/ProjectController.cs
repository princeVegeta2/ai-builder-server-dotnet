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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] AddProjectDto addProjectDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check if the project with the same name already exists for this user
            var projectExists = await _projectRepository.ProjectExistsForUserAsync(userId, addProjectDto.Name);

            if (projectExists)
            {
                return BadRequest(new { message = "A project with this name already exists." });
            }

            var project = _mapper.Map<Project>(addProjectDto);
            project.UserId = userId;
            await _projectRepository.AddProject(project);

            var response = new CreateProjectResponseDto
            {
                Message = "Project created successfully",
                ProjectId = project.Id
            };

            return Ok(response);
        }

    }
}
