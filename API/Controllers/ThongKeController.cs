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

            // Lấy danh sách hóa đơn trong tháng hiện tại
            var hoaDonIds = _context.HoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Select(h => h.HoaDonID)
                .ToList();

            // Lấy chi tiết hóa đơn có sản phẩm
            var chiTietSanPham = _context.ChiTietHoaDons
                .Where(c => c.SanPhamID != null && hoaDonIds.Contains(c.HoaDonID))
                .ToList();

            // Gom nhóm theo sản phẩm và join với bảng Products để lấy tên
            var grouped = chiTietSanPham
                .GroupBy(c => c.SanPhamID)
                .Join(_context.Products.ToList(),
                      g => g.Key,
                      p => p.ProductId,
                      (g, p) => new
                      {
                          ProductName = p.ProductName,
                          SoLuong = g.Sum(x => x.SoLuongSP)
                      })
                .ToList();

            // Tính tổng số lượng tất cả sản phẩm trong tháng
            var total = grouped.Sum(x => x.SoLuong);

            // Tính phần trăm cho từng sản phẩm, làm tròn 2 chữ số và sắp xếp giảm dần theo phần trăm
            var result = grouped
                .Select(x => new ThongKeSanPhamDTO
                {
                    ProductName = x.ProductName,
                    SoLuong = x.SoLuong,
                    PhanTram = (float)(total > 0 ? Math.Round((x.SoLuong * 100.0) / total, 2) : 0)
                })
                .OrderByDescending(x => x.PhanTram)
                .ToList();

            return Ok(result);
        }

        [HttpGet("SoLuongDichVu")]
        public IActionResult GetDichVuSoLuong()
        {
            var now = DateTime.Now;

            // Lấy danh sách hóa đơn trong tháng hiện tại
            var hoaDonIds = _context.HoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Select(h => h.HoaDonID)
                .ToList();

            // Lấy chi tiết hóa đơn có dịch vụ
            var chiTietDichVu = _context.ChiTietHoaDons
                .Where(c => c.DichVuID != null && hoaDonIds.Contains(c.HoaDonID))
                .ToList();

            // Gom nhóm theo dịch vụ
            var grouped = chiTietDichVu
                .GroupBy(c => c.DichVuID)
                .Join(_context.DichVus.ToList(),
                      g => g.Key,
                      p => p.DichVuID,
                      (g, p) => new
                      {
                          ServiceName = p.TenDichVu,
                          SoLuong = g.Sum(x => x.SoLuongSP)
                      })
                .ToList();

            // Tính tổng số lượng tất cả dịch vụ
            var total = grouped.Sum(x => x.SoLuong);

            // Tính phần trăm
            var result = grouped
                .Select(x => new ThongKeDichVuDTO
                {
                    ServiceName = x.ServiceName,
                    SoLuong = x.SoLuong,
                    PhanTram = (float)(total > 0 ? Math.Round((x.SoLuong * 100.0) / total, 2) : 0)
                })
                .OrderByDescending(x => x.PhanTram)
                .ToList();

            return Ok(result);
        }


        [HttpGet("TongTienThangNay")]
        public IActionResult GetTongTienThangNay()
        {
            var now = DateTime.Now;

            var tongTien = _context.HoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            return Ok(new { tongTien });
        }

        [HttpGet("TongTienHomNay")]
        public IActionResult GetTongTienHomNay()
        {
            var today = DateTime.Today;

            var tongTien = _context.HoaDons
                .Where(h => h.NgayTao.Date == today)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            return Ok(new { tongTien });
        }

        [HttpGet("DoanhThuThangHienTai")]
        public IActionResult GetDoanhThuTungNgayTrongThang()
        {
            var now = DateTime.Now;

            var result = _context.HoaDons
                .Where(h => h.NgayTao.Month == now.Month && h.NgayTao.Year == now.Year)
                .GroupBy(h => h.NgayTao.Date)
                .Select(g => new
                {
                    period = g.Key.Day,
                    value = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.period)
                .ToList();

            return Ok(result);
        }
        [HttpGet("DoanhThuNamHienTai")]
        public IActionResult GetDoanhThuTungThangTrongNam()
        {
            var now = DateTime.Now;

            var result = _context.HoaDons
                .Where(h => h.NgayTao.Year == now.Year)
                .GroupBy(h => h.NgayTao.Month)
                .Select(g => new
                {
                    period = g.Key,
                    value = g.Sum(x => x.TongTien)
                })
                .OrderBy(x => x.period)
                .ToList();

            return Ok(result);
        }

        [HttpGet("thongKeHoaDon")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDon()
        {
            var result = await _context.HoaDons
                    .Include(vc => vc.voucher)
                    .Include(dl => dl.ChiTietHoaDons)
                    .ThenInclude(ct => ct.DichVu)
                    .ToListAsync();

            return Ok(result);
        }
        [HttpGet("lichHenToday")]
        public async Task<IActionResult> GetSoLichHomNay()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var count = await _context.DatLiches
                .Where(dl => dl.ThoiGian >= today
                          && dl.ThoiGian < tomorrow
                          && dl.DatTruoc == true)
                .CountAsync();

            return Ok(new { SoLichHomNay = count });
        }
        [HttpGet("dvHoanThanhToday")]
        public async Task<IActionResult> GetThongKeDichVuHomNay()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var query = _context.ChiTietHoaDons
                .Where(ct => ct.DichVuID != null
                          && ct.HoaDon.NgayTao >= today
                          && ct.HoaDon.NgayTao < tomorrow);
            var soLuotDichVu = await query.CountAsync();


            return Ok(new{SoLuotDichVu = soLuotDichVu});
        }


    }
}
