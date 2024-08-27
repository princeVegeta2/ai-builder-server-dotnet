using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adding a new project to the "projects" table with UserID referencing the user
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task AddProject(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Check if the project with that name already exists tied to that user_id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public async Task<bool> ProjectExistsForUserAsync(int userId, string projectName)
        {
            return await _context.Projects
                .AnyAsync(p => p.UserId == userId && p.Name == projectName);
        }

        /// <summary>
        /// Retrieving a Project by using a userId and project name
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectByUserIdAndProjectName(int userId, string projectName)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId && p.Name == projectName)
                .FirstOrDefaultAsync();
        }

    }
}
