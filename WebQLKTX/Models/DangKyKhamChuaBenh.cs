using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class DangKyKhamChuaBenh
    {
        [Key]
        public int MaDangKy { get; set; }

        [Required]
        public DateTime NgayDangKy { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public string MaSV { get; set; } // Changed from int to string

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}

