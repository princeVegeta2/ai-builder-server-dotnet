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
    public class UserDataController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserDataController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("check-builder-access")]
        public async Task<IActionResult> CheckBuilderAccess()
        {
            // Get the user ID from JWT claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check if the user has builder access
            var hasBuilderAccess = await _userRepository.UserHasBuilderAccessAsync(userId);

            // Return the result
            return Ok(new { BuilderAccess = hasBuilderAccess });
        }

        [HttpGet("get-username")]
        public async Task<IActionResult> GetUsername()
        {
            // Get the user ID from JWT claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var username = await _userRepository.GetUsernameById(userId);

            if (username == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new { Username = username });
        }

        [HttpGet("get-email")]
        public async Task<IActionResult> GetEmail()
        {
            // Get the user ID from JWT claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var email = await _userRepository.GetEmailById(userId);

            if (email == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new { Email = email });
        }
    }
}
