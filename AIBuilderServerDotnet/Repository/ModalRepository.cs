using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class ModalRepository : IModalRepository
    {
        private ApplicationDbContext _context;

        public ModalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Methods for adding  different types of modals to the database. Each modal has it's own table
        public async Task AddColorModal(ColorModal colorModal)
        {
            await _context.ColorModals.AddAsync(colorModal);
            await _context.SaveChangesAsync();
        }

        public async Task AddLinkModal(LinkModal linkModal)
        {
            await _context.LinkModals.AddAsync(linkModal);
            await _context.SaveChangesAsync();
        }

        public async Task AddImageLinkModal(ImageLinkModal imageModal)
        {
            await _context.ImageLinkModals.AddAsync(imageModal);
            await _context.SaveChangesAsync();
        }

        public async Task AddPromptModal(PromptModal promptModal)
        {
            await _context.PromptModals.AddAsync(promptModal);
            await _context.SaveChangesAsync();
        }

        // Methods for deleting different types of modals from the database
        public async Task DeleteColorModal(ColorModal colorModal)
        {
            _context.ColorModals.Remove(colorModal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLinkModal(LinkModal linkModal)
        {
            _context.LinkModals.Remove(linkModal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageLinkModal(ImageLinkModal imageModal)
        {
            _context.ImageLinkModals.Remove(imageModal);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromptModal(PromptModal promptModal)
        {
            _context.PromptModals.Remove(promptModal);
            await _context.SaveChangesAsync();
        }

        // Methods for updating positions of modals
        public async Task UpdateModalPositionsGlobal(int widgetId, int position)
        {
            // Update ColorModals
            var colorModalsToUpdate = await _context.ColorModals
                .Where(cm => cm.WidgetId == widgetId && cm.Position > position)
                .ToListAsync();

            foreach (var colorModal in colorModalsToUpdate)
            {
                colorModal.Position -= 1;
            }

            // Update LinkModals
            var linkModalsToUpdate = await _context.LinkModals
                .Where(lm => lm.WidgetId == widgetId && lm.Position > position)
                .ToListAsync();

            foreach (var linkModal in linkModalsToUpdate)
            {
                linkModal.Position -= 1;
            }

            // Update ImageLinkModals
            var imageLinkModalsToUpdate = await _context.ImageLinkModals
                .Where(im => im.WidgetId == widgetId && im.Position > position)
                .ToListAsync();

            foreach (var imageLinkModal in imageLinkModalsToUpdate)
            {
                imageLinkModal.Position -= 1;
            }

            // Update PromptModals
            var promptModalsToUpdate = await _context.PromptModals
                .Where(pm => pm.WidgetId == widgetId && pm.Position > position)
                .ToListAsync();

            foreach (var promptModal in promptModalsToUpdate)
            {
                promptModal.Position -= 1;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // Methods to return all kinds of modals
        public async Task<ColorModal> GetColorModalByWidgetIdAndPosition(int widgetId, int position)
        {
            return await _context.ColorModals
                .Where(m => m.WidgetId == widgetId && m.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<LinkModal> GetLinkModalByWidgetIdAndPosition(int widgetId, int position)
        {
            return await _context.LinkModals
                .Where(m => m.WidgetId == widgetId && m.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<ImageLinkModal> GetImageLinkModalByWidgetIdAndPosition(int widgetId, int position)
        {
            return await _context.ImageLinkModals
                .Where(m => m.WidgetId == widgetId && m.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<PromptModal> GetPromptModalByWidgetIdAndPosition(int widgetId, int position)
        {
            return await _context.PromptModals
                .Where(m => m.WidgetId == widgetId && m.Position == position)
                .FirstOrDefaultAsync();
        }   
    }
}
