using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class YeuCau
    {
        [Key]
        public int MaYeuCau { get; set; }

        [Required]
        public string NoiDung { get; set; }

        [Required]
        public DateTime NgayGui { get; set; }

        [Required]
        public string TrangThai { get; set; } // "Chờ duyệt", "Đã duyệt", "Từ chối"

        public string MaSV { get; set; } // Changed from int to string

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}
