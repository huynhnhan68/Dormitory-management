using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using QuanLyKTX.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKTX.Controllers
{
    [Authorize]
    public class ThongBaoController : Controller
    {
        private readonly AppDbContext _context;

        public ThongBaoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ThongBaos.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TieuDe,NoiDung,NgayDang")] ThongBao thongBao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thongBao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thongBao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao == null)
                return NotFound();

            return View(thongBao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThongBao,TieuDe,NoiDung,NgayDang")] ThongBao thongBao)
        {
            if (id != thongBao.MaThongBao)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thongBao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongBaoExists(thongBao.MaThongBao))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(thongBao);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thongBao = await _context.ThongBaos
                .FirstOrDefaultAsync(m => m.MaThongBao == id);
            if (thongBao == null)
            {
                return NotFound();
            }

            return View(thongBao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var thongBao = await _context.ThongBaos.FirstOrDefaultAsync(m => m.MaThongBao == id);
            if (thongBao == null)
                return NotFound();

            return View(thongBao);
        }

        [HttpPost] // Đổi từ ActionName("Delete") thành HttpPost
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao == null)
                return NotFound();

            _context.ThongBaos.Remove(thongBao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThongBaoExists(int id)
        {
            return _context.ThongBaos.Any(e => e.MaThongBao == id);
        }
    }
}
