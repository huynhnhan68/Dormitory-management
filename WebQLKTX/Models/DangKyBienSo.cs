using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class DangKyBienSo
    {
        [Key]
        public int MaDangKy { get; set; }

        [Required]
        public string BienSoXe { get; set; }

        public string LoaiXe { get; set; }

        public string MaSV { get; set; } // Changed from int to string

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}
