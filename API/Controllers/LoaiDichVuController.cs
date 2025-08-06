using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using ClosedXML.Excel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiDichVuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoaiDichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoaiDichVus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiDichVu>>> GetLoaiDichVus()
        {
            return await _context.LoaiDichVus.ToListAsync();
        }

        // GET: api/LoaiDichVus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiDichVu>> GetLoaiDichVu(int id)
        {
            var loaiDichVu = await _context.LoaiDichVus.FindAsync(id);

            if (loaiDichVu == null)
            {
                return NotFound();
            }

            return loaiDichVu;
        }

        // PUT: api/LoaiDichVus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoaiDichVu(int id, LoaiDichVu loaiDichVu)
        {
            if (id != loaiDichVu.LoaiDichVuID)
            {
                return BadRequest();
            }

            _context.Entry(loaiDichVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiDichVuExists(id))
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

        // POST: api/LoaiDichVus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoaiDichVu>> PostLoaiDichVu(LoaiDichVu loaiDichVu)
        {
            _context.LoaiDichVus.Add(loaiDichVu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoaiDichVu", new { id = loaiDichVu.LoaiDichVuID }, loaiDichVu);
        }

        // DELETE: api/LoaiDichVus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiDichVu(int id)
        {
            var loaiDichVu = await _context.LoaiDichVus.FindAsync(id);
            if (loaiDichVu == null)
            {
                return NotFound();
            }

            _context.LoaiDichVus.Remove(loaiDichVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool LoaiDichVuExists(int id)
        {
            return _context.LoaiDichVus.Any(e => e.LoaiDichVuID == id);
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportLoaiDichVu(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file.");

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("File không hợp lệ, chỉ hỗ trợ .xlsx");

            var loaiDVList = new List<LoaiDichVu>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet("LoaiDichVu");
                    if (worksheet == null)
                        return BadRequest("Không tìm thấy sheet tên 'LoaiDichVu'.");

                    foreach (var row in worksheet.RowsUsed().Skip(1)) // Bỏ qua dòng tiêu đề
                    {
                        var tenLoai = row.Cell(1).GetString()?.Trim();
                        if (!string.IsNullOrEmpty(tenLoai))
                        {
                            loaiDVList.Add(new LoaiDichVu { TenLoai = tenLoai });
                        }
                    }
                }
            }

            _context.LoaiDichVus.AddRange(loaiDVList);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, count = loaiDVList.Count });
        }
    }
}
