using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class DatLich
    {
        public int DatLichID { get; set; }

        [Required]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        public DateTime ThoiGian { get; set; }

        [Required]
        public int ThoiLuong { get; set; }

        public int? DichVuID { get; set; } 

        public string? GhiChu { get; set; }

        public string TrangThai { get; set; } = "Chưa đến";
        public bool DaThanhToan { get; set; } = false;
        [ForeignKey("DichVuID")]
        [JsonIgnore]
        public DichVu? DichVu { get; set; }
    }
}
