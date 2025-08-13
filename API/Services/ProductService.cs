using API.Data;
using API.DTOs.Product;
using API.IService;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class ProductService : IProductService
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _env;

		public ProductService(ApplicationDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllAsync()
		{

			return await _context.Products
				.Include(p => p.ProductBatches)
				.Select(p => new ProductDTO
				{
					SanPhamId = p.ProductId,
					TenSP = p.ProductName,
					MoTa = p.Description,
					Gia = p.CurrentSellingPrice,
					SoLuong = p.ProductBatches
						.Where(b => b.ExpiryDate > DateTime.Now)
						.Sum(b => b.Quantity),
					LoaiSanPhamId = p.CategoryId,
					HinhAnh =  p.Images
				})
				.ToListAsync();
		}

		public async Task<ProductDTO> GetByIdAsync(int id)
		{
			var baseUrl = "https://localhost:7183/images/";

			var product = await _context.Products
				.Include(p => p.ProductBatches)
				.FirstOrDefaultAsync(p => p.ProductId == id);

			if (product == null) return null;

			return new ProductDTO
			{
				SanPhamId = product.ProductId,
				TenSP = product.ProductName,
				MoTa = product.Description,
				Gia = product.CurrentSellingPrice,
				SoLuong = product.ProductBatches
					.Where(b => b.ExpiryDate > DateTime.Now)
					.Sum(b => b.Quantity),
				LoaiSanPhamId = product.CategoryId,
				HinhAnh = string.IsNullOrEmpty(product.Images) ? null : baseUrl + product.Images
			};
		}

		public async Task<ProductDTO> CreateAsync(ProductCreateDTO dto)
		{
			var baseUrl = "https://localhost:7183/images/";
			var imageName = await SaveImageAsync(dto.Image);

			var product = new Product
			{
				ProductName = dto.ProductName,
				Description = dto.Description,
				CategoryId = dto.CategoryId,
				CurrentSellingPrice = dto.SellingPrice,
				Images = imageName,
				ProductBatches = new List<ProductBatch>
				{
					new ProductBatch
					{
						ImportPrice = dto.ImportPrice,
						Quantity = dto.Quantity,
						SupplierName = dto.SupplierName,
						ManufactureDate = dto.ManufactureDate,
						ExpiryDate = dto.ExpiryDate
					}
				}
			};

			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			return new ProductDTO
			{
				SanPhamId = product.ProductId,
				TenSP = product.ProductName,
				Gia = product.CurrentSellingPrice,
				MoTa = product.Description,
				SoLuong = product.ProductBatches.Sum(b => b.Quantity),
				LoaiSanPhamId = product.CategoryId,
				HinhAnh = string.IsNullOrEmpty(product.Images) ? null : baseUrl + product.Images
			};
		}

		public async Task<bool> UpdateAsync(int id, ProductUpdateDTO dto)
		{
			var product = await _context.Products
				.FirstOrDefaultAsync(p => p.ProductId == id);

			if (product == null) return false;

			product.ProductName = dto.ProductName;
			product.Description = dto.Description;
			product.CategoryId = dto.CategoryId;
			product.CurrentSellingPrice = dto.SellingPrice;

			if (dto.Image != null)
			{
				var imageName = await SaveImageAsync(dto.Image);
				product.Images = imageName;
			}

			_context.Products.Update(product);
			await _context.SaveChangesAsync();
			return true;
		}


		public async Task<bool> DeleteAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return false;

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return true;
		}

		private async Task<string> SaveImageAsync(IFormFile image)
		{
			if (image == null || image.Length == 0) return null;

			var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
			var filePath = Path.Combine(uploadsFolder, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await image.CopyToAsync(stream);
			}

			return fileName;
		}

		public async Task<IEnumerable<ProductDTO>> SearchByNameAsync(string name)
		{
			var baseUrl = "https://localhost:7183/images/";
			return await _context.Products
				.Include(p => p.ProductBatches)
				.Where(p => p.ProductName.Contains(name))
				.Select(p => new ProductDTO
				{
					SanPhamId = p.ProductId,
					TenSP = p.ProductName,
					Gia = p.CurrentSellingPrice,
					SoLuong = p.ProductBatches
						.Where(b => b.ExpiryDate > DateTime.Now)
						.Sum(b => b.Quantity),
					MoTa = p.Description,
					LoaiSanPhamId = p.CategoryId,
					HinhAnh = string.IsNullOrEmpty(p.Images) ? null : baseUrl + p.Images
				})
				.ToListAsync();
		}

		public async Task<IEnumerable<ProductDTO>> FilterByPriceAsync(decimal min, decimal max)
		{
			var baseUrl = "https://localhost:7183/images/";
			return await _context.Products
				.Include(p => p.ProductBatches)
				.Where(p => p.CurrentSellingPrice >= min && p.CurrentSellingPrice <= max)
				.Select(p => new ProductDTO
				{
					SanPhamId = p.ProductId,
					TenSP = p.ProductName,
					Gia = p.CurrentSellingPrice,
					SoLuong = p.ProductBatches
						.Where(b => b.ExpiryDate > DateTime.Now)
						.Sum(b => b.Quantity),
					MoTa = p.Description,
					LoaiSanPhamId = p.CategoryId,
					HinhAnh = string.IsNullOrEmpty(p.Images) ? null : baseUrl + p.Images
				})
				.ToListAsync();
		}
	}
}
