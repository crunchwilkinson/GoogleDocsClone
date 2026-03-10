using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GoogleDocsClone.Data;
using GoogleDocsClone.Models;
using GoogleDocsClone.Services;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;

namespace GoogleDocsClone.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService _documentsService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentsService documentsService, ILogger<DocumentsController> logger)
        {
            _documentsService = documentsService;
            _logger = logger;
        }

        // GET: Docs
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var documents = await _documentsService.GetDocumentsForUserAsync(userId);
            return View(documents);
        }

        
        // GET: Docs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Docs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] Document doc)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                await _documentsService.CreateDocumentAsync(doc, userId);
                _logger.LogInformation("User {UserId} successfully created a new document (Title: {Title}).", userId, doc.Title);
                TempData["SuccessMessage"] = "Document created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(doc);
        }

        // GET: Docs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doc = await _documentsService.GetDocumentByIdAsync(id.Value);
            if (doc == null)
            {
                return NotFound();
            }

            if (doc.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _logger.LogWarning("SECURITY: User {UserId} attempted to access/modify Document {DocumentId} which belongs to another user.", User.FindFirstValue(ClaimTypes.NameIdentifier), id);
                return NotFound();
            }

            return View(doc);
        }

        // POST: Docs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Document doc)
        {
            if (id != doc.Id)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updated = await _documentsService.UpdateDocumentAsync(doc, userId);
                    if (!updated)
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error occurred while User {UserId} was updating Document {DocumentId}.", userId, doc.Id);
                    if (!await DocExists(doc.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.LogInformation("User {UserId} successfully updated Document {DocumentId} (Title: {Title}).", userId, doc.Id, doc.Title);
                TempData["SuccessMessage"] = "Document updated successfully!";
                return RedirectToAction(nameof(Index));
            }
           
            return View(doc);
        }


            // POST: Docs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doc = await _documentsService.GetDocumentByIdAsync(id);
            var userId = GetCurrentUserId();

            if (doc != null)
            {
                // SECURITY CHECK: Ensure the logged-in user owns this document!
                if (doc.UserId != userId)
                {
                    _logger.LogWarning("SECURITY: User {UserId} attempted to access/modify Document {DocumentId} which belongs to another user.", userId, id);
                    return Unauthorized(); // Or NotFound()
                }

                await _documentsService.DeleteDocumentAsync(doc.Id, doc.UserId!);
                TempData["SuccessMessage"] = "Document deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DocExists(int id)
        {
            var doc = await _documentsService.GetDocumentByIdAsync(id);
            return doc != null;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}