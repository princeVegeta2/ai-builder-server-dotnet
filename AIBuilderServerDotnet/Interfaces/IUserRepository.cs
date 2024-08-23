using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Interfaces
{
    public interface IUserRepository
    {
        // Check if the email is already used
        Task<bool> UserExistsByEmailAsync(string email);

        // Check if the username is already used
        Task<bool> UserExistsByUsernameAsync(string username);

        // Add a user to the database 
        Task AddUserAsync(User user);

        // Get user by email
        Task<User> GetUserByEmailAsync(string email);

        // Check if the user has a builder access(by user id)
        Task<bool> UserHasBuilderAccessAsync(int userId);
    }
}
