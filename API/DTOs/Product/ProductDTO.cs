namespace API.DTOs.Product
{
    public class ProductDTO
    {
        public int SanPhamId { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public string HinhAnh { get; set; }
        public int LoaiSanPhamId { get; set; }

    }
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }  // Đây là ảnh upload
    }
}

