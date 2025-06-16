using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class HoaDon
    {
        public int HoaDonID { get; set; }
        public DateTime NgayTao { get; set; }
        public decimal TongTien { get; set; }
        public string? MaGiamGia { get; set; }
        public decimal? TongTienSauGiamGia { get; set; }
        public string HinhThucThanhToan { get; set; }
        public byte TrangThai { get; set; }
        public decimal? TienKhachDua { get; set; }
        public decimal? TienThoiLai { get; set; }

        public int NhanVienID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
