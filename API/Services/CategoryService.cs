using API.Data;
using API.DTOs.Product;
using API.IService;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDTO
                {
                    LoaiSanPhamId = c.CategoryId,
                    TenLoai = c.CategoryName
                }).ToListAsync();
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            return new CategoryDTO
            {
                LoaiSanPhamId = category.CategoryId,
                TenLoai = category.CategoryName
            };
        }

        public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
        {
            var category = new Category
            {
                CategoryName = dto.TenLoai
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            dto.LoaiSanPhamId = category.CategoryId;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.CategoryName = dto.TenLoai;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<CategoryDTO>> SearchByNameAsync(string ten)
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(ten))
                .Select(c => new CategoryDTO
                {
                    LoaiSanPhamId = c.CategoryId,
                    TenLoai = c.CategoryName
                }).ToListAsync();
        }



    }
}
