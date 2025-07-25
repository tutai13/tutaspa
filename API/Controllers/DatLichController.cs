using API.Data;
using API.DTOs.DatLich;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatLich>>> GetDatLich()
        {
            var result = await _context.datLiches
                    .Include(dl => dl.ChiTietDichVus)
                    .ThenInclude(ct => ct.DichVu)
                    .ToListAsync();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult DatLich([FromBody] DatLichDTO request)
        {
            int thoiLuong = 30; // Mặc định nếu không có dịch vụ
            List<DichVu> danhSachDichVu = new();

            if (request.DichVuIDs != null && request.DichVuIDs.Any())
            {
                danhSachDichVu = _context.DichVus
                    .Where(d => request.DichVuIDs.Contains(d.DichVuID))
                    .ToList();

                if (danhSachDichVu.Count != request.DichVuIDs.Count)
                    return BadRequest("Một hoặc nhiều dịch vụ không tồn tại");

                thoiLuong = danhSachDichVu.Sum(d => d.ThoiGian);
            }

            int soKhung = (int)Math.Ceiling(thoiLuong / 30.0);
            DateTime startTime = request.ThoiGian;

            // Kiểm tra từng khung giờ
            for (int i = 0; i < soKhung; i++)
            {
                var khung = startTime.AddMinutes(i * 30);

                int count = _context.datLiches
                    .Count(x =>
                        x.ThoiGian <= khung &&
                        x.ThoiGian.AddMinutes(x.ThoiLuong) > khung
                    );

                if (count >= 5)
                    return BadRequest($"Khung giờ {khung:HH:mm} đã đầy. Vui lòng chọn khung giờ khác.");
            }

            // Tạo bản ghi đặt lịch
            var datLich = new DatLich
            {
                SoDienThoai = request.SoDienThoai,
                ThoiGian = request.ThoiGian,
                ThoiLuong = thoiLuong,
                TrangThai = "Chưa đến",
                DaThanhToan = false,
                GhiChu = request.GhiChu
            };

            _context.datLiches.Add(datLich);
            _context.SaveChanges();

            // Nếu có dịch vụ, lưu ChiTietDatLich
            if (danhSachDichVu.Any())
            {
                foreach (var dv in danhSachDichVu)
                {
                    _context.chiTietDatLiches.Add(new ChiTietDatLich
                    {
                        DatLichID = datLich.DatLichID,
                        DichVuID = dv.DichVuID
                    });
                }

                _context.SaveChanges();
            }

            return Ok("🎉 Đặt lịch thành công!");
        }
        [HttpPut("{id}")]
        public IActionResult CapNhatDatLich(int id, [FromBody] DatLichDTO request)
        {
            var datLich = _context.datLiches.FirstOrDefault(d => d.DatLichID == id);
            if (datLich == null)
                return NotFound("Không tìm thấy lịch đặt");

            int thoiLuong = 30; // mặc định nếu không có dịch vụ
            List<DichVu> danhSachDichVu = new();

            if (request.DichVuIDs != null && request.DichVuIDs.Any())
            {
                danhSachDichVu = _context.DichVus
                    .Where(d => request.DichVuIDs.Contains(d.DichVuID))
                    .ToList();

                if (danhSachDichVu.Count != request.DichVuIDs.Count)
                    return BadRequest("Một hoặc nhiều dịch vụ không tồn tại");

                thoiLuong = danhSachDichVu.Sum(d => d.ThoiGian);
            }

            int soKhung = (int)Math.Ceiling(thoiLuong / 30.0);
            DateTime startTime = request.ThoiGian;

            // Kiểm tra trùng lịch (bỏ qua chính lịch đang sửa)
            for (int i = 0; i < soKhung; i++)
            {
                var khung = startTime.AddMinutes(i * 30);

                int count = _context.datLiches
                    .Count(x =>
                        x.DatLichID != id && // bỏ qua lịch hiện tại
                        x.ThoiGian <= khung &&
                        x.ThoiGian.AddMinutes(x.ThoiLuong) > khung
                    );

                if (count >= 5)
                    return BadRequest($"Khung giờ {khung:HH:mm} đã đầy. Vui lòng chọn khung giờ khác.");
            }

            // Cập nhật thông tin
            datLich.SoDienThoai = request.SoDienThoai;
            datLich.ThoiGian = request.ThoiGian;
            datLich.ThoiLuong = thoiLuong;
            datLich.GhiChu = request.GhiChu;

            // Xóa các chi tiết dịch vụ cũ
            var chiTietCu = _context.chiTietDatLiches
                .Where(c => c.DatLichID == datLich.DatLichID)
                .ToList();

            _context.chiTietDatLiches.RemoveRange(chiTietCu);

            // Thêm mới chi tiết dịch vụ
            if (danhSachDichVu.Any())
            {
                foreach (var dv in danhSachDichVu)
                {
                    _context.chiTietDatLiches.Add(new ChiTietDatLich
                    {
                        DatLichID = datLich.DatLichID,
                        DichVuID = dv.DichVuID
                    });
                }
            }

            _context.SaveChanges();

            return Ok("✅ Cập nhật đặt lịch thành công!");
        }
        [HttpPut("doitrangthai/{id}")]
        public IActionResult DoiTrangThai(int id)
        {
            var datLich = _context.datLiches.FirstOrDefault(d => d.DatLichID == id);
            if (datLich == null)
                return NotFound("Không tìm thấy lịch hẹn.");

            if (datLich.TrangThai == "Chưa đến")
            {
                datLich.TrangThai = "Đã đến";
            }
            else if (datLich.TrangThai == "Đã đến")
            {
                datLich.TrangThai = "Chưa đến";
            }
            else
            {
                return BadRequest("Trạng thái không hợp lệ để chuyển đổi.");
            }

            _context.SaveChanges();

            return Ok($"✅ Đã đổi trạng thái thành '{datLich.TrangThai}'.");
        }
        [HttpPut("capnhat-thanhtoan/{id}")]
        public async Task<IActionResult> CapNhatThanhToan(int id)
        {
            var datLich = await _context.datLiches.FindAsync(id);
            if (datLich == null)
                return NotFound();

            datLich.DaThanhToan = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("slots")]
        public IActionResult GetSlotInfo(DateTime ngay)
        {
            var startHour = 9;
            var endHour = 17;
            var result = new List<object>();

            var khungGioList = new List<DateTime>();
            for (int h = startHour; h <= endHour; h++)
            {
                khungGioList.Add(new DateTime(ngay.Year, ngay.Month, ngay.Day, h, 0, 0));
                khungGioList.Add(new DateTime(ngay.Year, ngay.Month, ngay.Day, h, 30, 0));
            }

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

