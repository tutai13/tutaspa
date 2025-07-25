using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Banggiadichvu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DichVuID { get; set; }

        public int? ThoiLuong { get; set; } // phút, nếu kiểu theo thời gian

        [Required]
        [Precision(18, 2)]
        public decimal Gia { get; set; }

        // 0 = tính theo thời gian, 1 = tính theo bước/quy trình
        [Required]
        public int KieuTinhGia { get; set; }

        [ForeignKey("DichVuID")]
        public DichVu? DichVu { get; set; }
        public bool IsVisible { get; set; } = true;

    }
}
