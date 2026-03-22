using System.ComponentModel.DataAnnotations;

namespace QuanLyKTX.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }

        [Required]
        public string TieuDe { get; set; }

        [Required]
        public string NoiDung { get; set; }

        [Required]
        public DateTime NgayDang { get; set; }
    }
}
