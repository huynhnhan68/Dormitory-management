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
    public class KhaiBaoBHYTController : Controller
    {
        private readonly AppDbContext _context;

        public KhaiBaoBHYTController(AppDbContext context)
        {
            _context = context;
        }

        // GET: KhaiBaoBHYT
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhaiBaoBHYTs.ToListAsync());
        }

        // GET: KhaiBaoBHYT/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhaiBaoBHYT/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhaiBaoBHYT khaiBaoBHYT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khaiBaoBHYT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khaiBaoBHYT);
        }

        // GET: KhaiBaoBHYT/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khaiBaoBHYT = await _context.KhaiBaoBHYTs.FindAsync(id);
            if (khaiBaoBHYT == null)
            {
                return NotFound();
            }
            return View(khaiBaoBHYT);
        }

        // POST: KhaiBaoBHYT/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhaiBaoBHYT khaiBaoBHYT)
        {
            if (id != khaiBaoBHYT.MaKhaiBao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khaiBaoBHYT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhaiBaoBHYTExists(khaiBaoBHYT.MaKhaiBao))
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
            return View(khaiBaoBHYT);
        }

        // GET: KhaiBaoBHYT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khaiBaoBHYT = await _context.KhaiBaoBHYTs
                .FirstOrDefaultAsync(m => m.MaKhaiBao == id);
            if (khaiBaoBHYT == null)
            {
                return NotFound();
            }

            return View(khaiBaoBHYT);
        }

        // GET: KhaiBaoBHYT/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khaiBaoBHYT = await _context.KhaiBaoBHYTs
                .FirstOrDefaultAsync(m => m.MaKhaiBao == id);
            if (khaiBaoBHYT == null)
            {
                return NotFound();
            }

            return View(khaiBaoBHYT);
        }

        // POST: KhaiBaoBHYT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khaiBaoBHYT = await _context.KhaiBaoBHYTs.FindAsync(id);
            if (khaiBaoBHYT != null)
            {
                _context.KhaiBaoBHYTs.Remove(khaiBaoBHYT);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KhaiBaoBHYTExists(int id)
        {
            return _context.KhaiBaoBHYTs.Any(e => e.MaKhaiBao == id);
        }
    }
}
