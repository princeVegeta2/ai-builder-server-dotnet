using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly string _jwtSecret;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            // Check if the user already exists
            if (_context.Users.Any(u => u.Email == signUpDto.Email))
            {
                return BadRequest("Email already in use");
            }else if (_context.Users.Any(u => u.Username == signUpDto.Username))
            {
                return BadRequest("Username already in use");
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password);

            // Create a new user
            var user = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = signUpDto.Password,
                CreatedAt = DateTime.UtcNow
            };

            // Add a new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered succesfully.");
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInDto signInDto)
        {
            // Find user by email
            var user = _context.Users.FirstOrDefault(u => u.Email == signInDto.Email);

            if (user == null)
            {
                return Unauthorized("Inavlid credentials.");
            }

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(signInDto.Password, user.PasswordHash))
            {
                return Unauthorized("Wrong password.");
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
