using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIBuilderServerDotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly string _jwtSecret;

        public AuthController(IUserRepository userRepository, IMapper mapper, string jwtSecret)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtSecret = jwtSecret ?? throw new ArgumentNullException(nameof(jwtSecret));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            // Check if the email already exists
            if (await _userRepository.UserExistsByEmailAsync(signUpDto.Email))
            {
                return BadRequest("Email already in use");
            }

            // Check if the username already exists
            if (await _userRepository.UserExistsByUsernameAsync(signUpDto.Username))
            {
                return BadRequest("Username already in use");
            }

            // Hash the password
            // var hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);

            // Create a new user
            var user = _mapper.Map<User>(signUpDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);
            user.CreatedAt = DateTime.UtcNow;
            /*
             * var user = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };
            */
            // Add a new user to the database
            await _userRepository.AddUserAsync(user);

            return Ok("User registered successfully.");
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new SignInResponseDto { Token = tokenString });
        }
    }
}
