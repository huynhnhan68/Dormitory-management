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
    public class SinhVienController : Controller
    {
        private readonly AppDbContext _context;

        public SinhVienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SinhVien
        public async Task<IActionResult> Index()
        {
            var sinhViens = await _context.SinhViens.ToListAsync();
            return View(sinhViens);
        }

        // GET: SinhVien/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null) return NotFound();

            return View(sinhVien);
        }

        // GET: SinhVien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SinhVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSV,HoTen,NgaySinh,GioiTinh,SDT,Email,DiaChi,MaPhong")] SinhVien sinhVien)
        {
            if (!ModelState.IsValid) return View(sinhVien);

            // Kiểm tra mã phòng có tồn tại không
            var phong = await _context.Phongs
                .Include(p => p.SinhViens)
                .FirstOrDefaultAsync(p => p.MaPhong == sinhVien.MaPhong);

            if (phong == null)
            {
                ModelState.AddModelError("MaPhong", "Phòng không tồn tại.");
                return View(sinhVien);
            }

            // Kiểm tra số lượng tối đa của phòng có hợp lệ không
            if (phong.SoLuongToiDa <= 0)
            {
                ModelState.AddModelError("MaPhong", "Số lượng tối đa của phòng phải là số dương.");
                return View(sinhVien);
            }

            // Kiểm tra số lượng sinh viên trong phòng
            if (phong.SinhViens.Count >= phong.SoLuongToiDa)
            {
                ModelState.AddModelError("MaPhong", "Phòng đã đầy, không thể thêm sinh viên.");
                return View(sinhVien);
            }

            // Thêm sinh viên vào cơ sở dữ liệu
            _context.Add(sinhVien);
            phong.SoLuongHienTai++;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: SinhVien/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null) return NotFound();

            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSV,HoTen,NgaySinh,GioiTinh,SDT,Email,DiaChi,MaPhong")] SinhVien sinhVien)
        {
            if (id != sinhVien.MaSV) return NotFound();

            if (!ModelState.IsValid) return View(sinhVien);

            try
            {
                var existingSinhVien = await _context.SinhViens.FindAsync(id);
                if (existingSinhVien == null) return NotFound();

                // Update only the fields that are allowed to be modified
                existingSinhVien.HoTen = sinhVien.HoTen;
                existingSinhVien.NgaySinh = sinhVien.NgaySinh;
                existingSinhVien.GioiTinh = sinhVien.GioiTinh;
                existingSinhVien.SDT = sinhVien.SDT;
                existingSinhVien.Email = sinhVien.Email;
                existingSinhVien.DiaChi = sinhVien.DiaChi;
                existingSinhVien.MaPhong = sinhVien.MaPhong;

                _context.Update(existingSinhVien);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SinhViens.Any(e => e.MaSV == id)) return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred while updating the student: {ex.Message}");
                return View(sinhVien);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SinhVien/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null) return NotFound();

            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null)
            {
                var phong = await _context.Phongs
                    .Include(p => p.SinhViens)
                    .FirstOrDefaultAsync(p => p.MaPhong == sinhVien.MaPhong);

                if (phong != null)
                {
                    phong.SoLuongHienTai--;
                }

                _context.SinhViens.Remove(sinhVien);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
