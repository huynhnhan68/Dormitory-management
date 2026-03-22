using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Data;
using QuanLyKTX.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKTX.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class PhongController : Controller
    {
        private readonly AppDbContext _context;

        public PhongController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Phong - Danh sách phòng
        [Authorize(Roles = "User, Admin")]

        public async Task<IActionResult> Index()
        {
            var danhSachPhong = await _context.Phongs
                .Include(p => p.SinhViens) // Đảm bảo lấy danh sách sinh viên trong phòng
                .ToListAsync();

            return View(danhSachPhong);
        }

        // GET: Phong/Details/5 - Xem chi tiết phòng
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var phong = await _context.Phongs
                .Include(p => p.SinhViens) // Lấy thông tin sinh viên trong phòng
                .FirstOrDefaultAsync(m => m.MaPhong == id);

            if (phong == null) return NotFound();

            return View(phong);
        }

        // GET: Phong/Create - Hiển thị form tạo phòng mới//
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Phong/Create - Thêm phòng mới
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhong,TenPhong,LoaiPhong,SoLuongToiDa")] Phong phong)
        {
            if (phong.SoLuongToiDa <= 0)
            {
                ModelState.AddModelError("SoLuongToiDa", "Số lượng tối đa phải lớn hơn 0.");
                return View(phong);
            }

            if (ModelState.IsValid)
            {
                _context.Add(phong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phong);
        }


        [HttpGet]
        public IActionResult CheckRoomCapacity(string maPhong)
        {
            var room = _context.Phongs
                .Include(p => p.SinhViens)
                .FirstOrDefault(p => p.MaPhong.ToString() == maPhong);
            if (room == null)
            {
                return Json(new { notFound = true });
            }

            var isFull = room.SinhViens.Count >= room.SoLuongToiDa;
            return Json(new { isFull });
        }



        // GET: Phong/Edit/5 - Hiển thị form chỉnh sửa phòng
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var phong = await _context.Phongs
                .Include(p => p.SinhViens) // Lấy danh sách sinh viên
                .FirstOrDefaultAsync(m => m.MaPhong == id);

            if (phong == null) return NotFound();

            // Gán lại số lượng sinh viên thực tế
            phong.SoLuongHienTai = phong.SinhViens.Count;

            return View(phong);
        }


        // POST: Phong/Edit/5 - Lưu chỉnh sửa phòng
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhong,TenPhong,LoaiPhong,SoLuongToiDa")] Phong phong)
        {
            if (id != phong.MaPhong) return NotFound();

            if (phong.SoLuongToiDa <= 0)
            {
                ModelState.AddModelError("SoLuongToiDa", "Số lượng tối đa phải lớn hơn 0.");
                return View(phong);
            }

            try
            {
                var phongDb = await _context.Phongs
                    .Include(p => p.SinhViens)
                    .FirstOrDefaultAsync(p => p.MaPhong == id);

                if (phongDb == null) return NotFound();

                int soLuongHienTai = phongDb.SinhViens.Count;
                if (soLuongHienTai > phong.SoLuongToiDa)
                {
                    ModelState.AddModelError("SoLuongToiDa", "Số lượng tối đa phải lớn hơn hoặc bằng số lượng sinh viên hiện tại.");
                    return View(phong);
                }

                phongDb.TenPhong = phong.TenPhong;
                phongDb.LoaiPhong = phong.LoaiPhong;
                phongDb.SoLuongToiDa = phong.SoLuongToiDa;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Phongs.Any(e => e.MaPhong == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: Phong/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var phong = await _context.Phongs
                .Include(p => p.SinhViens) // Kiểm tra xem có sinh viên trong phòng không
                .FirstOrDefaultAsync(m => m.MaPhong == id);

            if (phong == null) return NotFound();

            return View(phong);
        }

        // POST: Phong/Delete/5 - Xác nhận xóa phòng
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phong = await _context.Phongs
                .Include(p => p.SinhViens)
                .FirstOrDefaultAsync(p => p.MaPhong == id);

            if (phong != null)
            {
                if (phong.SinhViens.Any())
                {
                    ModelState.AddModelError("", "Không thể xóa phòng vì còn sinh viên ở trong.");
                    return View(phong);
                }

                _context.Phongs.Remove(phong);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
