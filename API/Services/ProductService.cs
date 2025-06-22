using System.Buffers.Text;
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
            var baseUrl = "https://localhost:7183/images/";
            return await _context.Product
                .Select(p => new ProductDTO
                {
                    SanPhamId = p.ProductId,
                    TenSP = p.ProductName,
                    SoLuong = p.Quantity,
                    MoTa = p.Description,
                    Gia = p.Price,
                    HinhAnh = string.IsNullOrEmpty(p.Images) ? null : baseUrl + p.Images,
                    LoaiSanPhamId = p.CategoryId
                })
                .ToListAsync();
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var baseUrl = "https://localhost:7183/images/";
            var product = await _context.Product.FindAsync(id);
            if (product == null) return null;

            return new ProductDTO
            {
                SanPhamId = product.ProductId,
                TenSP = product.ProductName,
                SoLuong = product.Quantity,
                MoTa = product.Description,
                Gia = product.Price,
                HinhAnh = string.IsNullOrEmpty(product.Images) ? null : baseUrl + product.Images,
                LoaiSanPhamId = product.CategoryId
            };
        }

        public async Task<ProductDTO> CreateAsync(ProductCreateDTO dto)
        {
            var baseUrl = "https://localhost:7183/images/";
            var imageName = await SaveImageAsync(dto.Image);

            var product = new Product
            {
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Images = imageName
            };

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDTO
            {
                SanPhamId = product.ProductId,
                TenSP = product.ProductName,
                SoLuong = product.Quantity,
                MoTa = product.Description,
                Gia = product.Price,
                HinhAnh = string.IsNullOrEmpty(product.Images) ? null : baseUrl + product.Images,
                LoaiSanPhamId = product.CategoryId
            };
        }


        public async Task<bool> UpdateAsync(int id, ProductCreateDTO dto)
        {
            //var baseUrl = "https://localhost:7183/images/";
            var product = await _context.Product.FindAsync(id);
            if (product == null) return false;

            product.ProductName = dto.ProductName;
            product.Quantity = dto.Quantity;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            if (dto.Image != null)
            {
                var imageName = await SaveImageAsync(dto.Image);
                product.Images = imageName;
            }

            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return false;

            _context.Product.Remove(product);
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
            return await _context.Product
                .Where(p => p.ProductName.Contains(name))
                .Select(p => new ProductDTO
                {
                    SanPhamId = p.ProductId,
                    TenSP = p.ProductName,
                    Gia = p.Price,
                    SoLuong = p.Quantity,
                    MoTa = p.Description,
                    LoaiSanPhamId = p.CategoryId,
                    HinhAnh = string.IsNullOrEmpty(p.Images) ? null : baseUrl + p.Images
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<ProductDTO>> FilterByPriceAsync(decimal min, decimal max)
        {
            var baseUrl = "https://localhost:7183/images/";
            return await _context.Product
                .Where(p => p.Price >= min && p.Price <= max)
                .Select(p => new ProductDTO
                {
                    SanPhamId = p.ProductId,
                    TenSP = p.ProductName,
                    Gia = p.Price,
                    SoLuong = p.Quantity,
                    MoTa = p.Description,
                    LoaiSanPhamId = p.CategoryId,
                    HinhAnh = string.IsNullOrEmpty(p.Images) ? null : baseUrl + p.Images
                })
                .ToListAsync();
        }




    }
}
