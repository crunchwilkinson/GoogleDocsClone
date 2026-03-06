using GoogleDocsClone.Data;
using GoogleDocsClone.Models;
using Microsoft.EntityFrameworkCore;

namespace GoogleDocsClone.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly ApplicationDbContext _context;
        public DocumentsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateDocumentAsync(Document doc, string userId)
        {
            doc.UserId = userId;
            doc.CreatedAt = DateTime.UtcNow;
            doc.UpdatedAt = DateTime.UtcNow;
            _context.Add(doc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(int id, string userId)
        {
            var doc = await _context.Docs.FindAsync(id);
            if (doc != null && doc.UserId == userId)
            {
                _context.Docs.Remove(doc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            var doc = await _context.Docs.FindAsync(id);
            if (doc != null) 
            {
                return doc;
            }
            return null;
        }
        

        public async Task<List<Document>> GetDocumentsForUserAsync(string userId)
        {
            return await _context.Docs.Where(d => d.UserId == userId).OrderByDescending(d => d.UpdatedAt).ToListAsync();
        }

        public async Task<bool> UpdateDocumentAsync(Document doc, string userId)
        {
            var existingDoc = await _context.Docs
                .FirstOrDefaultAsync(d => d.Id == doc.Id && d.UserId == userId);

            if (existingDoc == null)
            {
                return false;
            }

            existingDoc.Title = doc.Title;
            existingDoc.Content = doc.Content;
            existingDoc.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}