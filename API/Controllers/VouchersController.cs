using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VouchersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVoucher()
        {
            return await _context.Voucher.ToListAsync();
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(int id)
        {
            var voucher = await _context.Voucher.FindAsync(id);

            if (voucher == null)
                return NotFound();

            return voucher;
        }

        // PUT: api/Vouchers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(int id, Voucher voucher)
        {
            if (id != voucher.VoucherID)
                return BadRequest();

            _context.Entry(voucher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Vouchers
        [HttpPost]
        public async Task<ActionResult<Voucher>> PostVoucher(Voucher voucher)
        {
            voucher.MaCode = voucher.MaCode?.ToUpper() ?? string.Empty;

            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoucher", new { id = voucher.VoucherID }, voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await _context.Voucher.FindAsync(id);
            if (voucher == null)
                return NotFound();

            _context.Voucher.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Tìm kiếm theo mã code
        // GET: api/Vouchers/code?ma=VIP
        [HttpGet("code")]
        public async Task<ActionResult<IEnumerable<Voucher>>> SearchByCode([FromQuery] string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return await _context.Voucher.ToListAsync();

            return await _context.Voucher
                .Where(v => v.MaCode.ToLower().Contains(ma.ToLower()))
                .ToListAsync();
        }

        // ✅ Lọc theo giá trị giảm
        // GET: api/Vouchers/filter-value?min=10&max=100
        [HttpGet("filter-value")]
        public async Task<ActionResult<IEnumerable<Voucher>>> FilterByValue([FromQuery] float min, [FromQuery] float max)
        {
            return await _context.Voucher
                .Where(v => v.GiaTriGiam >= min && v.GiaTriGiam <= max)
                .ToListAsync();
        }

        // ✅ Lọc theo kiểu giảm giá
        // GET: api/Vouchers/filter-type?type=0
        [HttpGet("filter-type")]
        public async Task<ActionResult<IEnumerable<Voucher>>> FilterByType([FromQuery] byte type)
        {
            return await _context.Voucher
                .Where(v => v.KieuGiamGia == type)
                .ToListAsync();
        }

        private bool VoucherExists(int id)
        {
            return _context.Voucher.Any(e => e.VoucherID == id);
        }
    }
}
