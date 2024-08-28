using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIBuilderServerDotnet.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserDataController(IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpGet("check-builder-access")]
        public async Task<IActionResult> CheckBuilderAccess()
        {
            // Extract the user ID from the JWT token using JwtService
            var userId = _jwtService.GetUserIdFromToken(User);

            // Check if the user has builder access
            var hasBuilderAccess = await _userRepository.UserHasBuilderAccessAsync(userId);

            // Return the result
            return Ok(new { BuilderAccess = hasBuilderAccess });
        }

        [HttpGet("get-username")]
        public async Task<IActionResult> GetUsername()
        {
            // Extract the user ID from the JWT token using JwtService
            var userId = _jwtService.GetUserIdFromToken(User);

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
            // Extract the user ID from the JWT token using JwtService
            var userId = _jwtService.GetUserIdFromToken(User);

            var email = await _userRepository.GetEmailById(userId);

            if (email == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new { Email = email });
        }
    }
}
