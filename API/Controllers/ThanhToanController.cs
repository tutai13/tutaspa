using API.Data;
using API.DTOs.HoaDon;
using API.DTOs.ThongKe;
using API.Models;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using System.Text;
using Twilio.TwiML.Voice;
using DinkToPdf.Contracts;
using API.Extensions;
using DinkToPdf;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PayOS _payOS;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public ThanhToanController(ApplicationDbContext context, IConfiguration configuration, IConverter converter, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _context = context;
            string clientId = configuration["PayOSConfig:ClientId"] ?? throw new ArgumentNullException("ClientId");
            string apiKey = configuration["PayOSConfig:ApiKey"] ?? throw new ArgumentNullException("ApiKey");
            string checksumKey = configuration["PayOSConfig:ChecksumKey"] ?? throw new ArgumentNullException("ChecksumKey");

            _payOS = new PayOS(clientId, apiKey, checksumKey);
            _converter = converter;
            _env = env;
            _userManager = userManager;
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
                    qrCode = result.qrCode,
                    checkoutUrl = result.checkoutUrl,
                    orderId = orderId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi tạo link thanh toán: {ex.Message}");
            }
        }

        [HttpGet("by-user")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> GetHoaDonByUser()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var username = user?.UserName;
            var hoaDonList = await _context.HoaDons
                .Where(h => h.UserID == username)
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.DichVu)
                .OrderByDescending(h => h.NgayTao)
                .ToListAsync();

            return Ok(hoaDonList);
        }


        [HttpPost("tao-hoadon")]
        public async Task<IActionResult> TaoHoaDon([FromBody] HoaDonDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var tongTien = request.ChiTietHoaDon.Sum(x => x.ThanhTien);
                var voucherId = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.MaCode == request.MaGiamGia);

                var hoaDon = new HoaDon
                {
                    NgayTao = request.NgayTao,
                    HinhThucThanhToan = request.HinhThucThanhToan,
                    TrangThai = request.TrangThai,
                    TienKhachDua = request.TienKhachDua,
                    TienThoiLai = request.TienThoiLai,
                    TongTien = tongTien,
                    GiaTriGiam = request.tienGiam,
                    NhanVienID = request.NhanVienID,
                    UserID = request.UserID,
                    VoucherID = voucherId?.VoucherID,
                };

                _context.HoaDons.Add(hoaDon);
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
                    _context.ChiTietHoaDons.Add(chiTiet);
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

        [HttpGet("xuat-hoa-don")]
        public IActionResult XuatHoaDon()
        {
            // Tạo nội dung HTML hóa đơn
            var htmlContent = @"
            <html>
                <!DOCTYPE html><html><head><meta charset='UTF-8'>
                <head>
                    <style> body { font-family: Arial; } </style>
                </head>
                <body>
                    <h1 style='text-align:center'>TUTA SPA - HÓA ĐƠN</h1>
                    <p>Khách hàng: Nguyễn Văn A</p>
                    <p>Ngày: 25/07/2025</p>
                    <p>Dịch vụ: Massage Thư Giãn</p>
                    <p>Tổng tiền: 500,000đ</p>
                </body>
            </html>";

            // Trỏ đến wwwroot/libwkhtmltox/libwkhtmltox.dll
            var context = new CustomAssemblyLoadContext();
            var pathToLib = Path.Combine(_env.WebRootPath, "libs", "libwkhtmltox.dll");
            context.LoadUnmanagedLibrary(pathToLib);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent
                }
            }
            };

            var pdf = _converter.Convert(doc);

            return File(pdf, "application/pdf", "HoaDon.pdf");
        }


        [HttpGet("xuat-hoadon/{hoaDonId}")]
        public IActionResult XuatHoaDon(int hoaDonId)
        {
            var hoaDon = _context.HoaDons
                .Include(vc => vc.voucher)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.DichVu)
                .FirstOrDefault(h => h.HoaDonID == hoaDonId);

            if (hoaDon == null)
                return NotFound("Không tìm thấy hóa đơn");

            var html = new StringBuilder();

            html.Append(@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    * { font-family: Arial, sans-serif; font-size: 12px; }
                    h2 { text-align: center; margin-bottom: 5px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                    td { padding: 4px 0; }
                    .right { text-align: right; }
                    .center { text-align: center; }
                    .bold { font-weight: bold; }
                    .line { border-top: 1px dashed #000; margin: 5px 0; }
                </style>
            </head>
            <body>
            ");

            html.Append("<h2>TUTA SPA</h2>");
            html.Append("<div class='center'>--- HÓA ĐƠN ---</div>");
            html.Append($"<p><strong>Mã hóa đơn:</strong> {hoaDon.HoaDonID}<br>");
            html.Append($"<strong>Ngày:</strong> {hoaDon.NgayTao:dd/MM/yyyy HH:mm}<br>");
            html.Append($"<strong>NV:</strong> {hoaDon.NhanVienID}<br>");
            html.Append($"<strong>Khách:</strong> {hoaDon.UserID}</p>");
            html.Append("<div class='line'></div>");

            html.Append("<table>");
            html.Append("<tbody>");

            int stt = 1;
            foreach (var item in hoaDon.ChiTietHoaDons)
            {
                var ten = item.DichVu?.TenDichVu  ?? "Không rõ";
                var gia = item.DichVu?.Gia ?? 0;
                var soLuong = item.SoLuongSP;
                var thanhTien = item.ThanhTien;

                html.Append($@"
                <tr>
                    <td colspan='2'>{stt++}. {ten}</td>
                </tr>
                <tr>
                    <td class='right'>{gia:N0}đ x {soLuong}</td>
                    <td class='right'>{thanhTien:N0}đ</td>
                </tr>");
            }

            html.Append("</tbody></table>");
            html.Append("<div class='line'></div>");

            html.Append($@"
                <p class='right'>
                    <strong>Tổng: </strong> {hoaDon.TongTien:N0}đ<br>");

                        if (hoaDon.GiaTriGiam != 0)
                        {
                            html.Append($@"<strong>Giảm giá: </strong> {hoaDon.GiaTriGiam:N0}đ<br>");
                        }

                        html.Append($@"
                    <strong>Khách đưa: </strong> {hoaDon.TienKhachDua:N0}đ<br>
                    <strong>Thối lại: </strong> {hoaDon.TienThoiLai:N0}đ
                </p>
            ");

            html.Append("<div style='margin-top:15px;text-align:center;'>Cảm ơn quý khách!</div>");
            html.Append("</body></html>");

            // Load native lib
            var context = new CustomAssemblyLoadContext();
            var dllPath = Path.Combine(_env.WebRootPath, "libs", "libwkhtmltox.dll");
            context.LoadUnmanagedLibrary(dllPath);

            // Generate PDF
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = new PechkinPaperSize("80mm", "297mm"),
                    Margins = new MarginSettings { Top = 3, Bottom = 3, Left = 2, Right = 2 }
                },
                Objects = {
            new ObjectSettings
            {
                HtmlContent = html.ToString(),
                WebSettings = { DefaultEncoding = "utf-8" },
                LoadSettings = new LoadSettings { BlockLocalFileAccess = false }
            }
        }
            };

            var pdf = _converter.Convert(doc);
            return File(pdf, "application/pdf", $"HoaDon_{hoaDonId}.pdf");
        }


    }
}
