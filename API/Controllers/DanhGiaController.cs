using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DanhGiaController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Tạo đánh giá mới (chỉ nếu có hoá đơn hoàn tất)
        
        [HttpPost]
        public async Task<IActionResult> TaoDanhGia(DanhGia model)
        {
            try
            {
                _context.DanhGiass.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đánh giá đã được gửi chờ duyệt." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi nội bộ", error = ex.Message });
            }
        }


        // Danh sách đánh giá đã duyệt theo dịch vụ
        [HttpGet("dichvu/{maDichVu}")]
        public async Task<IActionResult> LayDanhGiaTheoDichVu(int maDichVu)
        {
            var danhGias = await _context.DanhGiass
                .Where(d => d.MaDichVu == maDichVu && d.DaDuyet)
                .Include(d => d.User)
                .OrderByDescending(d => d.NgayTao)
                .ToListAsync();

            return Ok(danhGias);
        }

        // Admin lấy tất cả đánh giá (chưa duyệt / đã duyệt)
        [HttpGet("admin")]
        public async Task<IActionResult> LayTatCa()
        {
            var danhGias = await _context.DanhGiass
                .Include(d => d.DichVu)
                .Include(d => d.User)
                .ToListAsync();

            return Ok(danhGias);
        }

        // Duyệt đánh giá
        [HttpPut("duyet/{id}")]
        public async Task<IActionResult> Duyet(int id)
        {
            var dg = await _context.DanhGiass.FindAsync(id);
            if (dg == null) return NotFound();

            dg.DaDuyet = true;
            await _context.SaveChangesAsync();
            return Ok("Đã duyệt đánh giá.");
        }

        // Xóa đánh giá
        [HttpDelete("{id}")]
        public async Task<IActionResult> Xoa(int id)
        {
            var dg = await _context.DanhGiass.FindAsync(id);
            if (dg == null) return NotFound();

            _context.DanhGiass.Remove(dg);
            await _context.SaveChangesAsync();
            return Ok("Đã xoá đánh giá.");
        }

        // LẤY TRUNG BÌNH SỐ SAO CỦA DỊCH VỤ
        [HttpGet("trungbinh/{maDichVu}")]
        public async Task<IActionResult> TinhTrungBinh(int maDichVu)
        {
            var trungBinh = await _context.DanhGiass
                .Where(d => d.MaDichVu == maDichVu && d.DaDuyet)
                .AverageAsync(d => (double?)d.SoSao) ?? 0;

            return Ok(trungBinh);
        }

        


    }

}
