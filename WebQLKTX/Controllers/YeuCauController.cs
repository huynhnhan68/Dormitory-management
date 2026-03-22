using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using QuanLyKTX.Models;
using System.Threading.Tasks;

namespace QuanLyKTX.Controllers
{
    [Authorize]
    public class YeuCauController : Controller
    {
        private readonly AppDbContext _context;

        public YeuCauController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.YeuCaus.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YeuCau yeuCau)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yeuCau);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yeuCau);
        }
        // GET: YeuCau/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var yeuCau = await _context.YeuCaus.FirstOrDefaultAsync(m => m.MaYeuCau == id);
            if (yeuCau == null) return NotFound();

            return View(yeuCau);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var yeuCau = await _context.YeuCaus.FindAsync(id);
            if (yeuCau == null) return NotFound();

            return View(yeuCau);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yeuCau = await _context.YeuCaus.FindAsync(id);
            _context.YeuCaus.Remove(yeuCau);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}