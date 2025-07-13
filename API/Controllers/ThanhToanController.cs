using API.Data;
using API.DTOs.HoaDon;
using API.DTOs.ThongKe;
using API.Migrations;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using Twilio.TwiML.Voice;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PayOS _payOS;

        public ThanhToanController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            string clientId = configuration["PayOSConfig:ClientId"] ?? throw new ArgumentNullException("ClientId");
            string apiKey = configuration["PayOSConfig:ApiKey"] ?? throw new ArgumentNullException("ApiKey");
            string checksumKey = configuration["PayOSConfig:ChecksumKey"] ?? throw new ArgumentNullException("ChecksumKey");

            _payOS = new PayOS(clientId, apiKey, checksumKey);
        }

        [HttpPost("create-link")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] PaymentDTO request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Request hoặc items không hợp lệ");
            }

            var items = request.Items.Select(i => new ItemData(i.Name, i.Quantity, i.Amount)).ToList();
            int orderId = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var paymentData = new PaymentData(
                orderId,
                request.TotalAmount,
                request.Description,
                items,
                request.cancelUrl,
                request.returnUrl
            );

            try
            {
                CreatePaymentResult result = await _payOS.createPaymentLink(paymentData);

                return Ok(new
                {
                    checkoutUrl = result.checkoutUrl,
                    orderId = orderId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi tạo link thanh toán: {ex.Message}");
            }
        }

        [HttpPost("tao-hoadon")]
        public async Task<IActionResult> TaoHoaDon([FromBody] HoaDonDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var tongTien = request.ChiTietHoaDon.Sum(x => x.ThanhTien);
                

                // Nếu có mã giảm giá, bạn có thể xử lý logic tính giảm tại đây

                var hoaDon = new HoaDon
                {
                    NgayTao = request.NgayTao,
                    //MaGiamGia = request.MaGiamGia,
                    HinhThucThanhToan = request.HinhThucThanhToan,
                    TrangThai = request.TrangThai,
                    TienKhachDua = request.TienKhachDua,
                    TienThoiLai = request.TienThoiLai,
                    TongTien = tongTien,
                   // TongTienSauGiamGia = tongSauGiamGia,
                    NhanVienID = request.NhanVienID,
                    UserID = request.UserID
                };

                _context.hoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                foreach (var item in request.ChiTietHoaDon)
                {
                    var chiTiet = new ChiTietHoaDon
                    {
                        HoaDonID = hoaDon.HoaDonID,
                        SanPhamID = item.SanPhamID,
                        DichVuID = item.DichVuID,
                        SoLuongSP = item.SoLuongSP, 
                        ThanhTien = item.ThanhTien
                    };
                    _context.chiTietHoaDons.Add(chiTiet);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Tạo hóa đơn thành công", hoaDonID = hoaDon.HoaDonID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Lỗi khi tạo hóa đơn", error = ex.Message });
            }
        }

    }
}
