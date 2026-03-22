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
    public class HoaDonTongController : Controller
    {
        private readonly AppDbContext _context;

        public HoaDonTongController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách hóa đơn
        public async Task<IActionResult> Index()
        {
            var hoaDonTongs = await _context.HoaDonTongs
                .Include(h => h.TienDienNuoc)
                .Select(h => new HoaDonTong
                {
                    MaHoaDon = h.MaHoaDon,
                    MaPhieuTienDienNuoc = h.MaPhieuTienDienNuoc,
                    MaPhong = h.MaPhong,
                    TienPhong = h.TienPhong,
                    TongTienDienNuoc = h.TienDienNuoc != null ? h.TienDienNuoc.TongTien : 0,
                    DaThanhToan = h.DaThanhToan
                })
                .ToListAsync();

            return View(hoaDonTongs);
        }

        // ✅ Xem chi tiết hóa đơn
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var hoaDonTong = await _context.HoaDonTongs
                .Include(h => h.TienDienNuoc)
                .ThenInclude(t => t.Phong)
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);

            if (hoaDonTong == null) return NotFound();

            hoaDonTong.TongTienDienNuoc = hoaDonTong.TienDienNuoc?.TongTien ?? 0;

            return View(hoaDonTong);
        }

        // ✅ Tạo mới hóa đơn (GET)
        public IActionResult Create()
        {
            ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
            return View();
        }

        // ✅ Tạo mới hóa đơn (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDonTong hoaDonTong)
        {
            if (ModelState.IsValid)
            {
                var phieuDienNuoc = await _context.TienDienNuocs
                    .FirstOrDefaultAsync(p => p.MaPhieu == hoaDonTong.MaPhieuTienDienNuoc);

                if (phieuDienNuoc == null)
                {
                    ModelState.AddModelError("", "Phiếu tiền điện nước không hợp lệ.");
                    ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
                    return View(hoaDonTong);
                }

                // Gán MaPhong và Tổng tiền điện nước từ phiếu
                hoaDonTong.MaPhong = phieuDienNuoc.MaPhong;
                hoaDonTong.TongTienDienNuoc = phieuDienNuoc.TongTien;

                // Lưu vào database
                _context.Add(hoaDonTong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
            return View(hoaDonTong);
        }


        // ✅ Chỉnh sửa hóa đơn (GET)
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var hoaDonTong = await _context.HoaDonTongs.FindAsync(id);
            if (hoaDonTong == null) return NotFound();

            ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
            return View(hoaDonTong);
        }

        // ✅ Chỉnh sửa hóa đơn (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HoaDonTong hoaDonTong)
        {
            if (id != hoaDonTong.MaHoaDon) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var phieuDienNuoc = await _context.TienDienNuocs
                        .FirstOrDefaultAsync(p => p.MaPhieu == hoaDonTong.MaPhieuTienDienNuoc);

                    if (phieuDienNuoc == null)
                    {
                        ModelState.AddModelError("", "Phiếu tiền điện nước không hợp lệ.");
                        ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
                        return View(hoaDonTong);
                    }

                    // Cập nhật lại MaPhong và Tổng tiền điện nước
                    hoaDonTong.MaPhong = phieuDienNuoc.MaPhong;
                    hoaDonTong.TongTienDienNuoc = phieuDienNuoc.TongTien;

                    _context.Update(hoaDonTong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.HoaDonTongs.Any(e => e.MaHoaDon == hoaDonTong.MaHoaDon))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DanhSachPhieuDienNuoc = _context.TienDienNuocs.ToList();
            return View(hoaDonTong);
        }

        // ✅ Xóa hóa đơn (GET)
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();

            var hoaDonTong = await _context.HoaDonTongs
                .Include(h => h.TienDienNuoc)
                .ThenInclude(t => t.Phong)
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);

            if (hoaDonTong == null) return NotFound();

            return View(hoaDonTong);
        }


        // ✅ Xóa hóa đơn (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDonTong = await _context.HoaDonTongs.FindAsync(id);
            if (hoaDonTong != null)
            {
                _context.HoaDonTongs.Remove(hoaDonTong);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }

}
