using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; }

        [Required]
        [MaxLength(100)]
        public string HoTen { get; set; }

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        [MaxLength(10)]
        public string GioiTinh { get; set; }

        [Required]
        [MaxLength(15)]
        public string SDT { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? DiaChi { get; set; }

        public int? MaPhong { get; set; }

        [ForeignKey(nameof(MaPhong))]
        public virtual Phong? Phong { get; set; }

    }
}
