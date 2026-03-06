using GoogleDocsClone.Models;

namespace GoogleDocsClone.Services
{
    public interface IDocumentsService
    {
        Task<List<Document>> GetDocumentsForUserAsync(string userId);
        Task<Document?> GetDocumentByIdAsync(int id);
        Task CreateDocumentAsync(Document doc, string userId);
        Task<bool> UpdateDocumentAsync(Document doc, string userId);
        Task DeleteDocumentAsync(int id, string userId);
    }
}