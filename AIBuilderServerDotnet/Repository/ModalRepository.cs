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
    }
}
