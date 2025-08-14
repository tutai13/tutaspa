using API.Data;
using API.DTOs.Product;
using API.IService;
using API.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ApplicationDbContext _context;

        public CategoryController(ICategoryService categoryService, ApplicationDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.LoaiSanPhamId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDTO dto)
        {
            var success = await _categoryService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string ten)
        {
            var result = await _categoryService.SearchByNameAsync(ten);
            return Ok(result);
        }
        [HttpPost("import-products")]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file.");

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("File không hợp lệ, chỉ hỗ trợ .xlsx");

            var categoryList = new List<Category>();
            var productList = new List<Product>();
            var batchList = new List<ProductBatch>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    // ===== Sheet Category =====
                    var sheetCategory = workbook.Worksheets.FirstOrDefault(s => s.Name == "Category");
                    if (sheetCategory != null)
                    {
                        foreach (var row in sheetCategory.RowsUsed().Skip(1))
                        {
                            var categoryName = row.Cell(1).GetString()?.Trim();
                            var maCategory = row.Cell(2).GetString()?.Trim();

                            if (!string.IsNullOrEmpty(maCategory) && !string.IsNullOrEmpty(categoryName))
                            {
                                categoryList.Add(new Category
                                {
                                    maCategory = maCategory,
                                    CategoryName = categoryName
                                });
                            }
                        }
                    }

                    // Lưu Category trước để có ID
                    if (categoryList.Any())
                    {
                        _context.Categories.AddRange(categoryList);
                        await _context.SaveChangesAsync();
                    }

                    // ===== Sheet Product =====
                    var sheetProduct = workbook.Worksheets.FirstOrDefault(s => s.Name == "Product");
                    if (sheetProduct != null)
                    {
                        foreach (var row in sheetProduct.RowsUsed().Skip(1))
                        {
                            try
                            {
                                var productName = row.Cell(1).GetString()?.Trim();
                                var description = row.Cell(2).GetString()?.Trim();
                                var importPrice = row.Cell(3).GetValue<decimal>();
                                var sellingPrice = row.Cell(4).GetValue<decimal>();
                                var quantity = row.Cell(5).GetValue<int>();
                                var supplierName = row.Cell(6).GetString()?.Trim();
                                var manufactureDate = row.Cell(7).GetDateTime();
                                var expiryDate = row.Cell(8).GetDateTime();
                                var hinhAnh = row.Cell(9).GetString()?.Trim();
                                var maProduct = row.Cell(10).GetString()?.Trim();

                                // Tìm Category theo mã
                                var category = await _context.Categories
                                    .FirstOrDefaultAsync(x => x.maCategory == maProduct);

                                if (category == null || string.IsNullOrEmpty(productName))
                                    continue;

                                // Tạo Product
                                var product = new Product
                                {
                                    ProductName = productName,
                                    Description = description,
                                    CategoryId = category.CategoryId,
                                    CurrentSellingPrice = sellingPrice,
                                    Images = hinhAnh,
                                    maProduct = maProduct
                                };
                                productList.Add(product);

                                // Tạo ProductBatch (Lô hàng đầu tiên)
                                var batch = new ProductBatch
                                {
                                    Product = product,
                                    ProductCode = maProduct,
                                    ImportPrice = importPrice,
                                    Quantity = quantity,
                                    SupplierName = supplierName,
                                    ManufactureDate = manufactureDate,
                                    ExpiryDate = expiryDate
                                };
                                batchList.Add(batch);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            if (productList.Any())
            {
                _context.Products.AddRange(productList);
                _context.ProductBatches.AddRange(batchList);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                success = true,
                categoryCount = categoryList.Count,
                productCount = productList.Count,
                batchCount = batchList.Count
            });
        }





    }
}
