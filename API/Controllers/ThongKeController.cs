using API.Data;
using API.DTOs.ThongKe;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThongKeController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("SoLuongSanPham")]
        public IActionResult GetSanPhamSoLuong()
        {
            var now = DateTime.Now;
            var hoaDonIds = _context.hoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Select(h => h.HoaDonID)
                .ToList(); 

            var chiTietSanPham = _context.chiTietHoaDons
                .Where(c => c.SanPhamID != null && hoaDonIds.Contains(c.HoaDonID))
                .ToList(); 

            var result = chiTietSanPham
                .GroupBy(c => c.SanPhamID)
                .Join(_context.Product.ToList(),
                      g => g.Key,
                      p => p.ProductId,
                      (g, p) => new ThongKeSanPhamDTO
                      {
                          ProductName = p.ProductName,
                          SoLuong = g.Sum(x => x.SoLuongSP)
                      })
                .ToList();

            return Ok(result);
        }

        [HttpGet("SoLuongDichVu")]
        public IActionResult GetDichVuSoLuong()
        {
            var now = DateTime.Now;
            var hoaDonIds = _context.hoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Select(h => h.HoaDonID)
                .ToList();

            var chiTietDichVu = _context.chiTietHoaDons
                .Where(c => c.DichVuID != null && hoaDonIds.Contains(c.HoaDonID))
                .ToList();

            var result = chiTietDichVu
                .GroupBy(c => c.DichVuID)
                .Join(_context.DichVus.ToList(),
                      g => g.Key,
                      p => p.DichVuID,
                      (g, p) => new ThongKeDichVuDTO
                      {
                          ServiceName = p.TenDichVu,
                          SoLuong = g.Sum(x => x.SoLuongSP)
                      })
                .ToList();

            return Ok(result);
        }

        [HttpGet("TongTienThangNay")]
        public IActionResult GetTongTienThangNay()
        {
            var now = DateTime.Now;

            var tongTien = _context.hoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            return Ok(new { tongTien });
        }

        [HttpGet("TongTienHomNay")]
        public IActionResult GetTongTienHomNay()
        {
            var today = DateTime.Today;

            var tongTien = _context.hoaDons
                .Where(h => h.NgayTao.Date == today)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            return Ok(new { tongTien });
        }

        [HttpGet("DoanhThuTungNgayTrongThang")]
        public IActionResult GetDoanhThuTungNgayTrongThang()
        {
            var now = DateTime.Now;

            var result = _context.hoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .GroupBy(h => h.NgayTao.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    TongTien = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.Ngay)
                .ToList();

            return Ok(result);
        }
        [HttpGet("thongKeHoaDon")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDon()
        {
            var result = await _context.hoaDons
                    .Include(dl => dl.ChiTietHoaDons)
                    .ThenInclude(ct => ct.DichVu)
                    .ToListAsync();

            return Ok(result);
        }

    }
}
