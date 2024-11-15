using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Archive.Data;
using E_Archive.Models;
using Microsoft.AspNetCore.Authorization;

namespace E_Archive.Controllers
{
    [Authorize]
    public class ArsipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArsipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Arsip
        public async Task<IActionResult> Index()
        {
            return View(await _context.arsips.ToListAsync());
        }

        // GET: Arsip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arsip = await _context.arsips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arsip == null)
            {
                return NotFound();
            }

            return View(arsip);
        }

        // GET: Arsip/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Arsip/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PathArsip,Owner")] Arsip arsip, IFormFile file)
        {
            if (file == null || file.Length == 0) {
                TempData["AlertMessage"] = "File Tidak Ada";
                return RedirectToAction(nameof(Create));
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arsip");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var username = User.Identity.Name;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            arsip.Owner = username;
            arsip.PathArsip = uniqueFileName;
            _context.Add(arsip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        // GET: Arsip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arsip = await _context.arsips.FindAsync(id);
            if (arsip == null)
            {
                return NotFound();
            }
            return View(arsip);
        }

        // POST: Arsip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PathArsip,Owner")] Arsip arsip)
        {
            if (id != arsip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arsip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArsipExists(arsip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(arsip);
        }

        // GET: Arsip/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arsip = await _context.arsips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arsip == null)
            {
                return NotFound();
            }

            if (arsip.Owner != User.Identity.Name)
            {
                TempData["AlertMessage"] = "Arsip Ini Bukan Milik Anda!";
                return RedirectToAction(nameof(Index));
            }

            return View(arsip);
        }

        // POST: Arsip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arsip = await _context.arsips.FindAsync(id);
            if (arsip != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/arsip", arsip.PathArsip);
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
                _context.arsips.Remove(arsip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArsipExists(int id)
        {
            return _context.arsips.Any(e => e.Id == id);
        }
    }
}
