using API.Data;
using API.DTOs.Product;
using API.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InventoryController : ControllerBase
	{
		private readonly IInventoryService _inventoryService;
		private readonly ApplicationDbContext _context;

		public InventoryController(IInventoryService inventoryService, ApplicationDbContext context)
		{
			_inventoryService = inventoryService;
			_context = context;
		}

		// ✅ Nhập hàng
		[HttpPost("import")]
		public async Task<IActionResult> Import([FromBody] InventoryActionDTO dto)
		{
			var result = await _inventoryService.ImportAsync(dto);
			if (!result)
				return BadRequest("Nhập hàng thất bại. Kiểm tra lại thông tin.");

			return Ok("Nhập hàng thành công.");
		}

		// ✅ Xuất hàng
		[HttpPost("export")]
		public async Task<IActionResult> Export([FromBody] InventoryActionDTO dto)
		{
			var result = await _inventoryService.ExportAsync(dto);
			if (!result)
				return BadRequest("Xuất hàng thất bại. Kiểm tra số lượng tồn kho.");

			return Ok("Xuất hàng thành công.");
		}

		// ✅ Lấy tất cả lịch sử
		[HttpGet("history")]
		public async Task<IActionResult> GetAllHistory()
		{
			var history = await _context.InventoryHistories
				.Include(x => x.Product)
				.OrderByDescending(x => x.Timestamp)
				.ToListAsync();

			return Ok(history);
		}

		// ✅ Lấy lịch sử nhập hàng
		[HttpGet("history/imports")]
		public async Task<IActionResult> GetImportHistory()
		{
			var importHistory = await _context.InventoryHistories
				.Include(x => x.Product)
				.Where(x => x.ActionType == "Import")
				.OrderByDescending(x => x.Timestamp)
				.ToListAsync();

			return Ok(importHistory);
		}

		// ✅ Lấy lịch sử xuất hàng
		[HttpGet("history/exports")]
		public async Task<IActionResult> GetExportHistory()
		{
			var exportHistory = await _context.InventoryHistories
				.Include(x => x.Product)
				.Where(x => x.ActionType == "Export")
				.OrderByDescending(x => x.Timestamp)
				.ToListAsync();

			return Ok(exportHistory);
		}
	}
}
