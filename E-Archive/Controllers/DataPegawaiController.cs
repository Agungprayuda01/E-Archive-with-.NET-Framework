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
using Microsoft.AspNetCore.Identity;

namespace E_Archive.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DataPegawaiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataPegawaiController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: DataPegawai
        public async Task<IActionResult> Index()
        {
            return View(await _context.pegawais.ToListAsync());
        }

        // GET: DataPegawai/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.pegawais
                .FirstOrDefaultAsync(m => m.Nik == id);
            if (pegawai == null)
            {
                return NotFound();
            }

            return View(pegawai);
        }

        // GET: DataPegawai/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataPegawai/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nik,Name,Alamat,Tanggallahir,Fotopath,Email")] Pegawai pegawai, IFormFile file)
        {
            var checkduplicate = await _context.pegawais.Where(a => a.Nik == pegawai.Nik).FirstOrDefaultAsync();
            if (checkduplicate != null) {
                TempData["AlertMessage"] = "Nik sudah terdaftar";
                return RedirectToAction(nameof(Create));
            }
            if (file == null || file.Length == 0) {
                TempData["AlertMessage"] = "Foto Tidak Ada";
                return RedirectToAction(nameof(Create));
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest("File must be an image with .jpg, .jpeg, .png, or .gif extension.");
            }

            long maxSize = 2 * 1024 * 1024; // 2 MB in bytes
            if (file.Length > maxSize)
            {
                return BadRequest("File size must not exceed 2 MB.");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            var username = User.Identity.Name;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            pegawai.Fotopath = uniqueFileName;
            _context.Add(pegawai);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DataPegawai/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.pegawais.FindAsync(id);
            if (pegawai == null)
            {
                return NotFound();
            }
            return View(pegawai);
        }

        // POST: DataPegawai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nik,Name,Alamat,Tanggallahir,Fotopath,Email,Role")] Pegawai pegawai)
        {
            var user = await _userManager.FindByEmailAsync(pegawai.Email);
            if (user == null) {
                TempData["AlertMessage"] = "Email Belum Terdaftar";
                return RedirectToAction(nameof(Edit));
            } 
            if (id != pegawai.Nik)
            {
                return NotFound();
            }
            if (!_roleManager.RoleExistsAsync("Admin").Result) { await _roleManager.CreateAsync(new IdentityRole("Admin")); }
            if (!_roleManager.RoleExistsAsync("Staff").Result) { await _roleManager.CreateAsync(new IdentityRole("Staff")); }
            if (ModelState.IsValid)
            {
                try
                {
                    var checkrole = await _userManager.IsInRoleAsync(user, "Admin");
                    var checkrole2 = await _userManager.IsInRoleAsync(user, "Staff");
                    if (checkrole)
                    {
                        var remove = await _userManager.RemoveFromRoleAsync(user, "Admin");
                        var add = await _userManager.AddToRoleAsync(user, pegawai.Role);
                    }
                    if (checkrole2)
                    {
                        var remove = await _userManager.RemoveFromRoleAsync(user, "Staff");
                        var add = await _userManager.AddToRoleAsync(user, pegawai.Role);
                    }
                    else
                    {
                        var add = await _userManager.AddToRoleAsync(user, pegawai.Role);
                    }
                    pegawai.Fotopath = pegawai.Fotopath;
                    _context.Update(pegawai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PegawaiExists(pegawai.Nik))
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
            return View(pegawai);
        }

        // GET: DataPegawai/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.pegawais
                .FirstOrDefaultAsync(m => m.Nik == id);
            if (pegawai == null)
            {
                return NotFound();
            }

            return View(pegawai);
        }

        // POST: DataPegawai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pegawai = await _context.pegawais.FindAsync(id);
            var user = await _userManager.FindByEmailAsync(pegawai.Email);
            if (pegawai != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", pegawai.Fotopath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                if (user != null) {
                    var remove = await _userManager.RemoveFromRoleAsync(user, pegawai.Role);
                }
                _context.pegawais.Remove(pegawai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PegawaiExists(string id)
        {
            return _context.pegawais.Any(e => e.Nik == id);
        }
    }
}
