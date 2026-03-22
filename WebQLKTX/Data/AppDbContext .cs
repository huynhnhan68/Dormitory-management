using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyKTX.Models;
using System;
using System.Linq;

namespace QuanLyKTX.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<DangKyBienSo> DangKyBienSos { get; set; }
        public DbSet<DangKyKhamChuaBenh> DangKyKhamChuaBenhs { get; set; }
        public DbSet<YeuCau> YeuCaus { get; set; }
        public DbSet<KhaiBaoBHYT> KhaiBaoBHYTs { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<TienDienNuoc> TienDienNuocs { get; set; }
        public DbSet<HoaDonTong> HoaDonTongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ SinhVien - Phong (1 Phòng có nhiều Sinh viên)
            modelBuilder.Entity<Phong>()
                .HasMany(p => p.SinhViens)
                .WithOne(sv => sv.Phong)
                .HasForeignKey(sv => sv.MaPhong)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa phòng nếu còn sinh viên

            // Quan hệ SinhVien - DangKyBienSo (1 Sinh viên có thể đăng ký biển số)
            modelBuilder.Entity<DangKyBienSo>()
                .HasOne(dk => dk.SinhVien)
                .WithMany()
                .HasForeignKey(dk => dk.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ SinhVien - DangKyKhamChuaBenh (1 Sinh viên có thể có nhiều lần khám chữa bệnh)
            modelBuilder.Entity<DangKyKhamChuaBenh>()
                .HasOne(dk => dk.SinhVien)
                .WithMany()
                .HasForeignKey(dk => dk.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ SinhVien - YeuCau (1 Sinh viên có thể gửi nhiều yêu cầu)
            modelBuilder.Entity<YeuCau>()
                .HasOne(yc => yc.SinhVien)
                .WithMany()
                .HasForeignKey(yc => yc.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ SinhVien - KhaiBaoBHYT (1 Sinh viên có thể khai báo BHYT nhiều lần)
            modelBuilder.Entity<KhaiBaoBHYT>()
                .HasOne(kb => kb.SinhVien)
                .WithMany()
                .HasForeignKey(kb => kb.MaSV)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<SinhVien>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var sinhVien = entry.Entity;
                if (sinhVien.MaPhong.HasValue)
                {
                    var phong = Phongs.Find(sinhVien.MaPhong.Value);
                    if (phong != null)
                    {
                        var soLuongHienTai = SinhViens.Count(sv => sv.MaPhong == phong.MaPhong);
                        if (soLuongHienTai >= phong.SoLuongToiDa)
                        {
                            throw new InvalidOperationException("Số lượng sinh viên trong phòng đã đạt tối đa.");
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
