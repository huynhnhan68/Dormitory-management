using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class HoaDonTong
    {
        [Key]
        public int MaHoaDon { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Không tự tăng
       


        [Required]
        public int MaPhieuTienDienNuoc { get; set; }

        [Required]
        public int MaPhong { get; set; }

        [Required]
        public decimal TienPhong { get; set; }

        [NotMapped]
        public decimal TongTienDienNuoc { get; set; }

        [NotMapped]
        public decimal TongHoaDon => TongTienDienNuoc + TienPhong;

        [Required]
        public bool DaThanhToan { get; set; } // Thêm thuộc tính này

        [ForeignKey("MaPhieuTienDienNuoc")]
        public TienDienNuoc? TienDienNuoc { get; set; }
    }
}
