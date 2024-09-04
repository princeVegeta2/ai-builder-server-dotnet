using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;
using AIBuilderServerDotnet.DTOs;

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

        /// <summary>
        /// Retrieves all projects for a specific user by userId.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of projects associated with the user.</returns>
        public async Task<IEnumerable<Project>> GetProjectsByUserId(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<ProjectDto> GetProjectDetails(int userId, string projectName)
        {
            var result = await (from project in _context.Projects
                                where project.UserId == userId && project.Name == projectName
                                select new ProjectDto
                                {
                                    ProjectName = project.Name,
                                    Pages = (from page in _context.Pages
                                             where page.ProjectId == project.Id
                                             select new PageDto
                                             {
                                                 PageName = page.Name,
                                                 Position = page.Position,
                                                 Widgets = (from widget in _context.Widgets
                                                            where widget.PageId == page.Id
                                                            select new WidgetDto
                                                            {
                                                                Type = widget.Type,
                                                                Position = widget.Position,
                                                                ColorModal = (from colorModal in _context.ColorModals
                                                                              where colorModal.WidgetId == widget.Id
                                                                              select new ColorModalDto
                                                                              {
                                                                                  Position = colorModal.Position,
                                                                                  Colors = (from color in _context.Colors
                                                                                            where color.ColorModalId == colorModal.Id
                                                                                            select new ColorValueDto
                                                                                            {
                                                                                                Color = color.ColorValue,
                                                                                                Position = color.Position
                                                                                            }).ToList()
                                                                              }).FirstOrDefault(),
                                                                LinkModal = (from linkModal in _context.LinkModals
                                                                             where linkModal.WidgetId == widget.Id
                                                                             select new LinkModalDto
                                                                             {
                                                                                 Position = linkModal.Position,
                                                                                 Links = (from link in _context.Links
                                                                                          where link.LinkModalId == linkModal.Id
                                                                                          select new LinkValueDto
                                                                                          {
                                                                                              Name = link.Name,
                                                                                              Url = link.Url,
                                                                                              Position = link.Position
                                                                                          }).ToList()
                                                                             }).FirstOrDefault(),
                                                                ImageLinkModal = (from imageLinkModal in _context.ImageLinkModals
                                                                                  where imageLinkModal.WidgetId == widget.Id
                                                                                  select new ImageLinkModalDto
                                                                                  {
                                                                                      Position = imageLinkModal.Position,
                                                                                      ImageLinks = (from imageLink in _context.ImageLinks
                                                                                                    where imageLink.ImageLinkModalId == imageLinkModal.Id
                                                                                                    select new ImageLinkValueDto
                                                                                                    {
                                                                                                        Url = imageLink.ImageUrl,
                                                                                                        Position = imageLink.Position
                                                                                                    }).ToList()
                                                                                  }).FirstOrDefault(),
                                                                PromptModal = (from promptModal in _context.PromptModals
                                                                               where promptModal.WidgetId == widget.Id
                                                                               select new PromptModalDto
                                                                               {
                                                                                   Position = promptModal.Position,
                                                                                   Prompt = (from prompt in _context.Prompts
                                                                                             where prompt.PromptModalId == promptModal.Id
                                                                                             select prompt.PromptValue).FirstOrDefault()
                                                                               }).FirstOrDefault()
                                                            }).ToList()
                                             }).ToList()
                                }).FirstOrDefaultAsync();

            return result;
        }



    }
}
