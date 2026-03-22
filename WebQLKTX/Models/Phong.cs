using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class Phong
    {
        [Key]
        public int MaPhong { get; set; }

        [Required]
        public string TenPhong { get; set; } = string.Empty;

        public string? LoaiPhong { get; set; }

        [Required]
        public int SoLuongToiDa { get; set; }

        public int SoLuongHienTai { get; set; } = 0;

        public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();

        public void UpdateSoLuongHienTai()
        {
            SoLuongHienTai = SinhViens.Count;
        }
    }
}
