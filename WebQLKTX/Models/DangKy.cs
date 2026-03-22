using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class DangKy
    {
        [Key]
        public int MaDangKy { get; set; }

        [Required]
        public DateTime NgayDangKy { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public string MaSV { get; set; } // Assuming MaSV is a string

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}

