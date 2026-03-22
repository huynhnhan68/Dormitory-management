using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using QuanLyKTX.Models;
using System.Threading.Tasks;

namespace QuanLyKTX.Controllers
{
    [Authorize]
    public class DangKyKhamChuaBenhController : Controller
    {
        private readonly AppDbContext _context;

        public DangKyKhamChuaBenhController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DangKyKhamChuaBenhs.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DangKyKhamChuaBenh dangKyKhamChuaBenh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dangKyKhamChuaBenh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dangKyKhamChuaBenh);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dangKyKhamChuaBenh = await _context.DangKyKhamChuaBenhs.FindAsync(id);
            if (dangKyKhamChuaBenh == null) return NotFound();

            return View(dangKyKhamChuaBenh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DangKyKhamChuaBenh dangKyKhamChuaBenh)
        {
            if (id != dangKyKhamChuaBenh.MaDangKy) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(dangKyKhamChuaBenh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dangKyKhamChuaBenh);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dangKyKhamChuaBenh = await _context.DangKyKhamChuaBenhs
                .FirstOrDefaultAsync(m => m.MaDangKy == id);
            if (dangKyKhamChuaBenh == null) return NotFound();

            return View(dangKyKhamChuaBenh);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dangKyKhamChuaBenh = await _context.DangKyKhamChuaBenhs
                .FirstOrDefaultAsync(m => m.MaDangKy == id);
            if (dangKyKhamChuaBenh == null) return NotFound();

            return View(dangKyKhamChuaBenh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dangKyKhamChuaBenh = await _context.DangKyKhamChuaBenhs.FindAsync(id);
            _context.DangKyKhamChuaBenhs.Remove(dangKyKhamChuaBenh);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
