using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKTX.Models
{
    public class TienDienNuoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Không tự động tăng
        public int MaPhieu { get; set; }

        [Required]
        public int MaPhong { get; set; }

        [Required]
        public DateTime NgayTaoPhieu { get; set; }

        [Required]
        public int SoDien { get; set; }

      

        [Required]
        public int SoNuoc { get; set; }

        [Required]
        
        public decimal TongTien { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal GiaDien { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal GiaNuoc { get; set; }

        public decimal CalculateTotal()
        {
            return SoDien * GiaDien + SoNuoc * GiaNuoc;
        }
    }
}
