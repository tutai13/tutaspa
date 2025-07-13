namespace API.DTOs.DatLich
{
    public class DatLichDTO
    {
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime ThoiGian { get; set; } 
        public int? DichVuID { get; set; }
        public string? GhiChu { get; set; }
    }
}
