using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AIBuilderServerDotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthController(IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            // Check if the email already exists
            if (await _userRepository.UserExistsByEmailAsync(signUpDto.Email))
            {
                return BadRequest(new { error = "Email already in use" });
            }

            // Check if the username already exists
            if (await _userRepository.UserExistsByUsernameAsync(signUpDto.Username))
            {
                return BadRequest(new { error = "Username already in use" });
            }

            // Create a new user
            var user = _mapper.Map<User>(signUpDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);
            user.CreatedAt = DateTime.UtcNow;

            // Add a new user to the database
            await _userRepository.AddUserAsync(user);

            return Ok(new { message = "User registered successfully." });
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(signInDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email.");
            }

            if (!BCrypt.Net.BCrypt.Verify(signInDto.Password, user.PasswordHash))
            {
                return Unauthorized("Wrong password.");
            }

            // Use the JwtService to generate the token
            var token = _jwtService.GenerateToken(user.Id, user.Email, signInDto.StaySignedIn);

            return Ok(new SignInResponseDto { Token = token });
        }
    }
}
