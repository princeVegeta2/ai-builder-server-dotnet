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
    }
}
