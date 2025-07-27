using API.Models;
using API.Models.Chat;
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
        public DbSet<Category> Categorys { get; set; }

		public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<DanhGia> DanhGiass{ get; set; }
        public DbSet<Banggiadichvu> BangGiaDichVus { get; set; }
        public DbSet<HoaDon> hoaDons { get; set; }
        public DbSet<ChiTietHoaDon> chiTietHoaDons { get; set; }
        public DbSet<API.Models.Voucher> Voucher { get; set; } = default!;


        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

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


            modelBuilder.Entity<ChatSession>(entity =>
            {
                entity.ToTable("ChatSessions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SessionId).IsUnique();
                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.AdminId);
                entity.HasIndex(e => e.StartTime);
                entity.HasIndex(e => e.Status);

                entity.Property(e => e.CustomerName).HasMaxLength(100);
                entity.Property(e => e.AdminName).HasMaxLength(100);
                entity.Property(e => e.CustomerIP).HasMaxLength(45); // IPv6 max length
                entity.Property(e => e.UserAgent).HasMaxLength(500);
            });

            // ChatMessageEntity Configuration
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessages");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.MessageId).IsUnique();
                entity.HasIndex(e => e.SessionId);
                entity.HasIndex(e => e.FromUserId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.IsFromAdmin);
                entity.HasIndex(e => e.Status);

                entity.Property(e => e.FromUserName).HasMaxLength(100);
                entity.Property(e => e.Message).HasMaxLength(2000);

                // Foreign Key Relationship
                entity.HasOne(m => m.ChatSession)
                      .WithMany(s => s.Messages)
                      .HasForeignKey(m => m.SessionId)
                      .HasPrincipalKey(s => s.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}

