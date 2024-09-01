using AIBuilderServerDotnet.Data;
using AIBuilderServerDotnet.Interfaces;
using AIBuilderServerDotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace AIBuilderServerDotnet.Repository
{
    public class ModalValuesRepository : IModalValuesRepository
    {
        private ApplicationDbContext _context;

        public ModalValuesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implementing methods to adding entries to colors, links, image_links and prompts tables
        public async Task AddColorValue(Color color)
        {
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
        }

        public async Task AddImageLinkValue(ImageLink imageLink)
        {
            await _context.ImageLinks.AddAsync(imageLink);
            await _context.SaveChangesAsync();
        }

        public async Task AddLinkValue(Link link)
        {
            await _context.Links.AddAsync(link);
            await _context.SaveChangesAsync();
        }

        public async Task AddPromptValue(Prompt prompt)
        {
            await _context.Prompts.AddAsync(prompt);
            await _context.SaveChangesAsync();
        }

        // Implementing methods for getting instances from the database
        public async Task<Color> GetColorByColorModalIdAndPosition(int colorModalId, int position)
        {
            return await _context.Colors
                .Where(c => c.ColorModalId == colorModalId && c.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<Link> GetLinkByLinkModalIdAndPosition(int linkModalId, int position)
        {
            return await _context.Links
                .Where(l => l.LinkModalId == linkModalId && l.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<ImageLink> GetImageLinkByImageLinkModalIdAndPosition(int imageLinkModalId, int position)
        {
            return await _context.ImageLinks
                .Where(il => il.ImageLinkModalId == imageLinkModalId && il.Position == position)
                .FirstOrDefaultAsync();
        }

        public async Task<Prompt> GetPromptByPromptModalIdAndPosition(int promptModalId, int position)
        {
            return await _context.Prompts
                .Where(p => p.PromptModalId == promptModalId && p.Position == position)
                .FirstOrDefaultAsync();
        }

        // Implementing methods for checking if the value exists for current position in the current modal
        public async Task<bool> CheckIfColorValueExistsForAModal(string colorValue, int colorModalId)
        {
            return await _context.Colors
                .AnyAsync(c => c.ColorValue == colorValue && c.ColorModalId == colorModalId);
                
        }

        public async Task<bool> CheckIfLinkNameExistsForAModal(string linkName, int linkModalId)
        {
            return await _context.Links
                .AnyAsync(l => l.Name == linkName && l.LinkModalId == linkModalId);
        }

        public async Task<bool> CheckIfUrlExistsForAModal(string url, int linkModalId)
        {
            return await _context.Links
                .AnyAsync(l => l.Url == url && l.LinkModalId == linkModalId);
        }

        public async Task<bool> CheckIfImageImageUrlExistsForAModal(string imageUrl, int linkModalId)
        {
            return await _context.ImageLinks
                .AnyAsync(il => il.ImageUrl == imageUrl && il.ImageLinkModalId == linkModalId);
        }

        public async Task<bool> CheckIfPromptExistsForAModal(string prompt, int promptModalId)
        {
            return await _context.Prompts
                .AnyAsync(p => p.PromptValue == prompt && p.PromptModalId == promptModalId);
        }

        // Implementing methods to check if the position already exists for the current modal
        public async Task<bool> PositionAlreadyExistsForColorModalId(int position, int modalId)
        {
            return await _context.Colors
                .AnyAsync(c => c.Position == position && c.ColorModalId == modalId);
        }

        public async Task<bool> PositionAlreadyExistsForLinkModalId(int position, int modalId)
        {
            return await _context.Links
                .AnyAsync(l => l.Position == position && l.LinkModalId == modalId);
        }

        public async Task<bool> PositionAlreadyExistsForImageLinkModalId(int position, int modalId)
        {
            return await _context.ImageLinks
                .AnyAsync(il => il.Position == position && il.ImageLinkModalId == modalId);
        }

        public async Task<bool> PositionAlreadyExistsForPrompModalId(int position, int modalId)
        {
            return await _context.Prompts
                .AnyAsync(p => p.Position == position && p.PromptModalId == modalId);
        }


        // Implementing methods to update values in the database for modal values tables
        public async Task UpdateColorValue(string colorValue, int colorModalId, int position)
        {
            await _context.Colors
                .Where(c => c.ColorModalId == colorModalId && c.Position == position)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.ColorValue, colorValue));
        }

        public async Task UpdateLinkName(string linkName, int linkModalId, int position)
        {
            await _context.Links
                .Where(l => l.LinkModalId == linkModalId && l.Position == position)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Name, linkName));
        }

        public async Task UpdateLinkUrl(string linkUrl, int linkModalId, int position)
        {
            await _context.Links
                .Where(l => l.LinkModalId == linkModalId && l.Position == position)
                .ExecuteUpdateAsync(s => s.SetProperty(l => l.Url, linkUrl));
        }

        public async Task UpdateImageLinkUrl(string imageLinkUrl, int imageLinkModalId, int position)
        {
            await _context.ImageLinks
                .Where(il => il.ImageLinkModalId == imageLinkModalId && il.Position == position)
                .ExecuteUpdateAsync(s => s.SetProperty(il => il.ImageUrl, imageLinkUrl));
        }

        public async Task UpdatePromptValue(string prompt, int promptModalId, int position)
        {
            await _context.Prompts
                .Where(p => p.PromptModalId == promptModalId && p.Position == position)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.PromptValue, prompt));
        }

        // Implementing methods for Deleting instances in "colors", "links", "image_links" and "prompts" tables
        public async Task DeleteColor(Color color)
        {
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLink(Link link)
        {
            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageLink(ImageLink imageLink)
        {
            _context.ImageLinks.Remove(imageLink);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrompt(Prompt prompt)
        {
            _context.Prompts.Remove(prompt);
            await _context.SaveChangesAsync();
        }

    }
}
