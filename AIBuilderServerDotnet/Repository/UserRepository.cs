using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;

        /// <summary>
        /// Dependancy injection
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a filled user model to the database. Use for SignUp
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds a user in the Users table by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Finds if a user with that email already exists in the table
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Finds if a user with that Username already exists in the table
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// Finds if a user has builder access
        /// </summary>
        /// <param name="builderAccess"></param>
        /// <returns></returns>
        public async Task<bool> UserHasBuilderAccessAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.BuilderAccess ?? false;
        }

        /// <summary>
        /// Finds the username of a user by userId(using JWT)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetUsernameById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Username;
        }

        /// <summary>
        /// Finds the email of a user by userId(using JWT)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetEmailById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Email;
        }
    }
}
