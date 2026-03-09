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

namespace GoogleDocsClone.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        // GET: Docs
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var documents = await _documentsService.GetDocumentsForUserAsync(userId!);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                await _documentsService.CreateDocumentAsync(doc, userId);
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DocExists(doc.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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
            
            if (doc != null)
            {
                // SECURITY CHECK: Ensure the logged-in user owns this document!
                if (doc.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return Unauthorized(); // Or NotFound()
                }

                await _documentsService.DeleteDocumentAsync(doc.Id, doc.UserId!);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DocExists(int id)
        {
            var doc = await _documentsService.GetDocumentByIdAsync(id);
            return doc != null;
        }
    }
}