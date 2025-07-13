using API.Data;
using API.DTOs.DatLich;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatLichController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DatLichController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult DatLich([FromBody] DatLichDTO request)
        {
            int thoiLuong = 30;

            if (request.DichVuID.HasValue)
            {
                var dv = _context.DichVus.Find(request.DichVuID.Value);
                if (dv == null)
                    return NotFound("Dịch vụ không tồn tại");

                thoiLuong = dv.ThoiGian;
            }

            int soKhung = thoiLuong / 30;
            DateTime startTime = request.ThoiGian;

            // Kiểm tra từng khung giờ bị chiếm
            for (int i = 0; i < soKhung; i++)
            {
                var khung = startTime.AddMinutes(i * 30);

                int count = _context.datLiches
                    .Count(x =>
                        x.ThoiGian <= khung &&
                        x.ThoiGian.AddMinutes(x.ThoiLuong) > khung
                    );

                if (count >= 5)
                {
                    return BadRequest($"Khung giờ {khung:HH:mm} đã đầy");
                }
            }

            var datLich = new DatLich
            {
                SoDienThoai = request.SoDienThoai,
                ThoiGian = request.ThoiGian,
                ThoiLuong = thoiLuong,
                DichVuID = request.DichVuID,
                TrangThai = "Chưa đến"
            };

            _context.datLiches.Add(datLich);
            _context.SaveChanges();

            return Ok("Đặt lịch thành công");
        }

        [HttpGet("slots")]
        public IActionResult GetSlotInfo(DateTime ngay)
        {
            var startHour = 9;
            var endHour = 17;
            var result = new List<object>();

            // Tạo danh sách khung giờ
            var khungGioList = new List<DateTime>();
            for (int h = startHour; h <= endHour; h++)
            {
                khungGioList.Add(new DateTime(ngay.Year, ngay.Month, ngay.Day, h, 0, 0));
                khungGioList.Add(new DateTime(ngay.Year, ngay.Month, ngay.Day, h, 30, 0));
            }

            // Lấy tất cả lịch trong ngày
            var lich = _context.datLiches
                .Where(x => x.ThoiGian.Date == ngay.Date)
                .ToList();

            foreach (var khung in khungGioList)
            {
                var count = lich.Count(x =>
                    x.ThoiGian <= khung &&
                    x.ThoiGian.AddMinutes(x.ThoiLuong) > khung
                );

                result.Add(new
                {
                    khungGio = khung.ToString("HH:mm"),
                    conLai = Math.Max(0, 5 - count)
                });
            }

            return Ok(result);
        }
        
    }
}
