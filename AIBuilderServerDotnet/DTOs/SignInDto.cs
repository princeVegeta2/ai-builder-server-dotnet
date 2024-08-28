using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace AIBuilderServerDotnet.DTOs
{
    public class SignInDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool StaySignedIn { get; set; } = false;
    }
}
