using System.Security.Claims;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, bool staySignedIn);
        int GetUserIdFromToken(ClaimsPrincipal user);
    }
}
