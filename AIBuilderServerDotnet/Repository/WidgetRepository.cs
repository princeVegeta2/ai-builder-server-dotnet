using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class WidgetRepository : IWidgetRepository
    {
        private ApplicationDbContext _context;
        public WidgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a widget to the widgets table
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        public async Task AddWidget(Widget widget)
        {
            await _context.Widgets.AddAsync(widget);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a widget from the widgets table
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        public async Task DeleteWidget(Widget widget)
        {
            _context.Widgets.Remove(widget);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Decrements positions for all widgets for a page, where the position was greater than the position
        /// of a deleted widget
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="widgetPosition"></param>
        /// <returns></returns>
        public async Task UpdateWidgetPositionsForAPage(int pageId, int widgetPosition)
        {
            // Fetch all widgets on the page where the position is greater than the deleted widget's position
            var widgetsToUpdate = await _context.Widgets
                .Where(w => w.PageId == pageId && w.Position > widgetPosition)
                .ToListAsync();

            // Decrement the position of each widget
            foreach (var widget in widgetsToUpdate)
            {
                widget.Position -= 1;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds and returns a Widget by finding it with page id and position
        /// </summary>
        /// <param name="pageid"></param>
        /// <param name="widgetPosition"></param>
        /// <returns></returns>
        public async Task<Widget> GetWidgetByPageIdAndPosition(int pageid, int widgetPosition)
        {
            return await _context.Widgets
                .Where(w => w.PageId == pageid && w.Position == widgetPosition)
                .FirstOrDefaultAsync();
        }
    }
}
