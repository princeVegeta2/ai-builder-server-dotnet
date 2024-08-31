using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class PageRepository : IPageRepository
    {
        private ApplicationDbContext _context;

        public PageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Task to add a new page instance to the "pages" table
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task AddPage(Page page)
        {
            await _context.Pages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Task to remove a page from the "pages" table
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task DeletePage(Page page)
        {
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds if a page with the same name tied to that project already exists
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public async Task<bool> PageExistsForProjectAsync(int projectId, string pageName)
        {
            return await _context.Pages
                .AnyAsync(p => p.ProjectId == projectId && p.Name == pageName);
        }

        /// <summary>
        /// Finds a page by ProjectId and page name
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public async Task<Page> GetPageByProjectIdAndName(int projectId, string pageName)
        {
            return await _context.Pages
                .Where(p => p.ProjectId == projectId && p.Name == pageName)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates a page with new data. Can be reused
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task UpdatePage(Page page)
        {
            // Mark the entity as modified
            _context.Pages.Update(page);

            await _context.SaveChangesAsync();
        }
    }
}
