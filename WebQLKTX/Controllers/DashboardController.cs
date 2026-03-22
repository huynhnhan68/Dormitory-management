using Microsoft.AspNetCore.Mvc;
using WebQLKTX.Models;
using System.Linq;
using QuanLyKTX.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebQLKTX.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalStudents = _context.SinhViens.Count();
            var totalRooms = _context.Phongs.Count();

            var totalRequests = _context.YeuCaus.Count();
            var totalAnnouncements = _context.ThongBaos.Count();
            var totalHealthDeclarations = _context.KhaiBaoBHYTs.Count();
            var totalMedicalRegistrations = _context.DangKyKhamChuaBenhs.Count();

            var studentStats = _context.SinhViens
                .GroupBy(s => s.GioiTinh)
                .Select(g => new StudentStat { Gender = g.Key, Count = g.Count() })
                .ToList();

            return View(new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalRooms = totalRooms,
                TotalMaleStudents = _context.SinhViens.Count(s => s.GioiTinh == "Nam"),
                TotalFemaleStudents = _context.SinhViens.Count(s => s.GioiTinh == "Nữ"),

                TotalRequests = totalRequests,
                TotalAnnouncements = totalAnnouncements,
                TotalHealthDeclarations = totalHealthDeclarations,
                TotalMedicalRegistrations = totalMedicalRegistrations,
                StudentStats = studentStats
            });
        }
    }
}
