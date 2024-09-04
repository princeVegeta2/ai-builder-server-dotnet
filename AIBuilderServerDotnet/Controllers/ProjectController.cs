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
        private readonly IJwtService _jwtService;

        public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] AddProjectDto addProjectDto)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

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

        [HttpGet("user-projects")]
        public async Task<IActionResult> GetUserProjectNames()
        {
            var userId = _jwtService.GetUserIdFromToken(User);
            var projects = await _projectRepository.GetProjectsByUserId(userId);

            if (projects == null || !projects.Any())
            {
                return BadRequest(new { message = "No projects found for this user." });
            }

            var projectNames = projects.Select(p => p.Name).ToList();

            return Ok(projectNames);
        }

        [HttpGet("load-project")]
        public async Task<IActionResult> GetProjectDetails([FromQuery] string projectName)
        {
            var userId = _jwtService.GetUserIdFromToken(User);

            var projectDetails = await _projectRepository.GetProjectDetails(userId, projectName);

            if (projectDetails == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            return Ok(projectDetails);
        }



    }
}
