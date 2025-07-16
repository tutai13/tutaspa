using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Hosting;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DichVuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public DichVuController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DichVu>>> GetDichVus()
        {
            return await _context.DichVus
                .Where(dv => dv.TrangThai == 1)
                .ToListAsync();
        }
        // ✅ (Tùy chọn) Trả về tất cả dịch vụ (admin dùng)
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDichVus()
        {
            var dichVus = await _context.DichVus
                .Include(dv => dv.LoaiDichVu) // Join với bảng LoaiDichVu
                .Select(dv => new
                {
                    dv.DichVuID,
                    dv.TenDichVu,
                    dv.Gia,
                    dv.ThoiGian,
                    dv.MoTa,
                    dv.HinhAnh,
                    dv.NgayTao,
                    dv.TrangThai,
                    dv.LoaiDichVuID,
                    dv.LoaiDichVu.TenLoai // Lấy tên loại
                })
                .ToListAsync();

            return Ok(dichVus);
        }

        // GET: api/DichVus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DichVu>> GetDichVu(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);

            if (dichVu == null)
            {
                return NotFound();
            }

            return dichVu;
        }
        // GET: api/DichVu/loai
        [HttpGet("loai")]
        public async Task<ActionResult<IEnumerable<object>>> GetLoaiDichVus()
        {
            var loaiList = await _context.LoaiDichVus
                .Select(ldv => new {
                    ldv.LoaiDichVuID,
                    ldv.TenLoai
                })
                .ToListAsync();

            return Ok(loaiList);
        }
        // PUT: api/DichVus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDichVu(int id, DichVu dichVu)
        {
            if (id != dichVu.DichVuID)
            {
                return BadRequest();
            }

            _context.Entry(dichVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DichVuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DichVus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DichVu>> PostDichVu(DichVu dichVu)
        {
            dichVu.NgayTao =DateTime.Now;
            _context.DichVus.Add(dichVu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDichVu", new { id = dichVu.DichVuID }, dichVu);
        }
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<DichVu>>> SearchByName(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten))
                return BadRequest("Vui lòng nhập tên dịch vụ cần tìm.");

            var keyword = ten.ToLower();

            var result = await _context.DichVus
                .Where(d => d.TenDichVu.ToLower().Contains(keyword))
                .ToListAsync();

            if (result == null || result.Count == 0)
                return NotFound("Không tìm thấy dịch vụ phù hợp.");

            return Ok(result);
        }
        // GET: api/DichVu/filter-by-price?min=50000&max=100000
        [HttpGet("filter-by-price")]
        public async Task<ActionResult<IEnumerable<DichVu>>> FilterByPrice(decimal min, decimal max)
        {
            if (min < 0 || max < 0 || min > max)
            {
                return BadRequest(new { Message = "Khoảng giá không hợp lệ." });
            }
            var result = await _context.DichVus
                .Where(d => d.Gia >= min && d.Gia <= max)
                .ToListAsync();

            if (result == null || result.Count == 0)
                return NotFound("Không tìm thấy dịch vụ trong khoảng giá đã nhập.");

            return Ok(result);
        }
        // PUT: api/DichVu/toggle-status/5
        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> ToggleTrangThai(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);

            if (dichVu == null)
            {
                return NotFound(new { Message = "Không tìm thấy dịch vụ." });
            }

            // Giả sử: 1 = Hoạt động, 2 = Ngừng hoạt động
            dichVu.TrangThai = (dichVu.TrangThai == 1) ? 2 : 1;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Cập nhật trạng thái thành công.",
                    TrangThaiMoi = dichVu.TrangThai == 1 ? "Hoạt động" : "Ngừng hoạt động"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi cập nhật trạng thái.", Error = ex.Message });
            }
        }
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Không có file ảnh." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");

            // Nếu file trùng tên thì tự động thêm hậu tố - copy
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{originalFileName}{extension}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            int count = 1;
            while (System.IO.File.Exists(filePath))
            {
                fileName = $"{originalFileName}_{count}{extension}";
                filePath = Path.Combine(uploadsFolder, fileName);
                count++;
            }

            // Lưu ảnh vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }
        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.DichVuID == id);
        }
    }
}
