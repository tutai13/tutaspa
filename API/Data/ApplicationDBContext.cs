using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> User { get; set; } 
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        // --------------------------
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<LoaiDichVu> LoaiDichVus { get; set; }
        public DbSet<Product> Product { get; set; }
		public DbSet<ProductBatch> ProductBatches { get; set; }
		public DbSet<Category> Categorys { get; set; }

		public DbSet<InventoryHistory> InventoryHistories { get; set; }

        public DbSet<HoaDon> hoaDons { get; set; }
        public DbSet<ChiTietHoaDon> chiTietHoaDons { get; set; }
        public DbSet<API.Models.Voucher> Voucher { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.Property(e => e.Gia)
                      .HasColumnType("decimal(18,2)");
            });

			// Giá bán của sản phẩm
			modelBuilder.Entity<Product>()
				.Property(p => p.CurrentSellingPrice)
				.HasColumnType("decimal(18,2)");

			// Thiết lập mối quan hệ 1-n Product → ProductBatch
			modelBuilder.Entity<ProductBatch>()
				.Property(b => b.ImportPrice)
				.HasColumnType("decimal(18,2)");

			modelBuilder.Entity<Product>()
				.HasMany(p => p.ProductBatches)
				.WithOne(b => b.Product)
				.HasForeignKey(b => b.ProductId)
				.OnDelete(DeleteBehavior.Cascade); // hoặc Restrict nếu muốn cứng hơn

			// Quan hệ InventoryHistory → ProductBatch
			modelBuilder.Entity<InventoryHistory>()
				.HasOne(h => h.Batch)
				.WithMany()
				.HasForeignKey(h => h.BatchId)
				.OnDelete(DeleteBehavior.Restrict);
		}
    }

}

