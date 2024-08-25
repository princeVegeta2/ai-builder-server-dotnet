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
            // Get the user ID from JWT claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Map AddProjectDto to Project
            var project = _mapper.Map<Project>(addProjectDto);
            project.UserId = userId; // Set the UserId using the value from the JWT token

            // Add the project to the database
            await _projectRepository.AddProject(project);

            return Ok("Project created successfully.");
        }
    }
}
