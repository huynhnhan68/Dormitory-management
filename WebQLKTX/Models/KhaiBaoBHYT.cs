using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class KhaiBaoBHYT
    {
        [Key]
        public int MaKhaiBao { get; set; }

        [Required]
        public string MaTheBHYT { get; set; }

        [Required]
        public DateTime NgayCap { get; set; }

        [Required]
        public DateTime NgayHetHan { get; set; }

        public string MaSV { get; set; } // Changed from int to string

        [ForeignKey("MaSV")]
        public SinhVien? SinhVien { get; set; }
    }
}
