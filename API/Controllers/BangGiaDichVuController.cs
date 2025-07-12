using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BangGiaDichVuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BangGiaDichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Lấy bảng giá theo thời gian (KieuTinhGia == 0)
        [HttpGet("GetGiaTheoThoiGian/{dichVuID}")]
        public async Task<IActionResult> GetGiaTheoThoiGian(int dichVuID)
        {
            var result = await _context.BangGiaDichVus
                .Where(g => g.DichVuID == dichVuID && g.KieuTinhGia == 0)
                .OrderBy(g => g.ThoiLuong)
                .Select(g => new
                {
                    g.Id,
                    g.ThoiLuong,
                    g.Gia
                })
                .ToListAsync();

            return Ok(result);
        }

        // ✅ Lấy toàn bộ bảng giá của một dịch vụ (mọi kiểu)
        [HttpGet("GetGiaByDichVu/{dichVuID}")]
        public async Task<IActionResult> GetGiaByDichVu(int dichVuID)
        {
            var result = await _context.BangGiaDichVus
                .Where(g => g.DichVuID == dichVuID)
                .OrderBy(g => g.ThoiLuong)
                .ToListAsync();

            return Ok(result);
        }

        // ✅ Thêm bảng giá mới
        [HttpPost("AddGiaDichVu")]
        public async Task<IActionResult> AddGiaDichVu([FromBody] Banggiadichvu model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.BangGiaDichVus.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        // ✅ Cập nhật bảng giá
        [HttpPut("UpdateGiaDichVu/{id}")]
        public async Task<IActionResult> UpdateGiaDichVu(int id, [FromBody] Banggiadichvu model)
        {
            var gia = await _context.BangGiaDichVus.FindAsync(id);
            if (gia == null) return NotFound();

            gia.ThoiLuong = model.ThoiLuong;
            gia.Gia = model.Gia;
            gia.KieuTinhGia = model.KieuTinhGia;
            gia.DichVuID = model.DichVuID;

            await _context.SaveChangesAsync();
            return Ok(gia);
        }

        // ✅ Xoá bảng giá
        [HttpDelete("DeleteGiaDichVu/{id}")]
        public async Task<IActionResult> DeleteGiaDichVu(int id)
        {
            var gia = await _context.BangGiaDichVus.FindAsync(id);
            if (gia == null) return NotFound();

            _context.BangGiaDichVus.Remove(gia);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xoá thành công" });
        }
        // ✅ Lấy toàn bộ bảng giá (cho admin)
        // Controllers/BangGiaDichVuController.cs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.BangGiaDichVus
                .Include(bg => bg.DichVu)
                .OrderBy(bg => bg.DichVuID)
                .ThenBy(bg => bg.ThoiLuong)
                .Select(bg => new
                {
                    bg.Id,
                    bg.DichVuID,
                    TenDichVu = bg.DichVu != null ? bg.DichVu.TenDichVu : null,
                    bg.ThoiLuong,
                    bg.Gia,
                    bg.KieuTinhGia
                })
                .ToListAsync();

            return Ok(list);
        }

      


    }
}
