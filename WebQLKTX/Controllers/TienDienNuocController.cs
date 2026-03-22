using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using QuanLyKTX.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyKTX.Controllers
{
    [Authorize]
    public class TienDienNuocController : Controller
    {
        private readonly AppDbContext _context;

        public TienDienNuocController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TienDienNuoc
        public async Task<IActionResult> Index()
        {
            var tienDienNuocList = await _context.TienDienNuocs.Include(t => t.Phong).ToListAsync();
            return View(tienDienNuocList);
        }

        // GET: TienDienNuoc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienDienNuoc = await _context.TienDienNuocs
                .Include(t => t.Phong)
                .FirstOrDefaultAsync(m => m.MaPhieu == id);
            if (tienDienNuoc == null)
            {
                return NotFound();
            }

            return View(tienDienNuoc);
        }

        // GET: TienDienNuoc/Create
        public IActionResult Create()
        {
            ViewData["MaPhong"] = new SelectList(_context.Phongs, "MaPhong", "TenPhong");
            return View();
        }

        // POST: TienDienNuoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TienDienNuoc tienDienNuoc)
        {
            if (ModelState.IsValid)
            {
                tienDienNuoc.TongTien = tienDienNuoc.CalculateTotal();
                _context.Add(tienDienNuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhong"] = new SelectList(_context.Phongs, "MaPhong", "TenPhong", tienDienNuoc.MaPhong);
            return View(tienDienNuoc);
        }

        // GET: TienDienNuoc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienDienNuoc = await _context.TienDienNuocs.FindAsync(id);
            if (tienDienNuoc == null)
            {
                return NotFound();
            }
            ViewData["MaPhong"] = new SelectList(_context.Phongs, "MaPhong", "TenPhong", tienDienNuoc.MaPhong);
            return View(tienDienNuoc);
        }

        // POST: TienDienNuoc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TienDienNuoc tienDienNuoc)
        {
            if (id != tienDienNuoc.MaPhieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tienDienNuoc.TongTien = tienDienNuoc.CalculateTotal();
                    _context.Update(tienDienNuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TienDienNuocExists(tienDienNuoc.MaPhieu))
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
            ViewData["MaPhong"] = new SelectList(_context.Phongs, "MaPhong", "TenPhong", tienDienNuoc.MaPhong);
            return View(tienDienNuoc);
        }

        // GET: TienDienNuoc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienDienNuoc = await _context.TienDienNuocs
                .Include(t => t.Phong)
                .FirstOrDefaultAsync(m => m.MaPhieu == id);
            if (tienDienNuoc == null)
            {
                return NotFound();
            }

            return View(tienDienNuoc);
        }

        // POST: TienDienNuoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tienDienNuoc = await _context.TienDienNuocs.FindAsync(id);
            _context.TienDienNuocs.Remove(tienDienNuoc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TienDienNuocExists(int id)
        {
            return _context.TienDienNuocs.Any(e => e.MaPhieu == id);
        }
    }
}
