using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop2.Models;

public partial class LaptopShopContext : DbContext
{
    public LaptopShopContext()
    {
    }

    public LaptopShopContext(DbContextOptions<LaptopShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbBrand> TbBrands { get; set; }

    public virtual DbSet<TbCategoryNew> TbCategoryNews { get; set; }

    public virtual DbSet<TbCategoryProduct> TbCategoryProducts { get; set; }

    public virtual DbSet<TbContact> TbContacts { get; set; }

    public virtual DbSet<TbCustomer> TbCustomers { get; set; }

    public virtual DbSet<TbImage> TbImages { get; set; }

    public virtual DbSet<TbMenu> TbMenus { get; set; }

    public virtual DbSet<TbMenuAdmin> TbMenuAdmins { get; set; }

    public virtual DbSet<TbNews> TbNews { get; set; }

    public virtual DbSet<TbOrder> TbOrders { get; set; }

    public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; }

    public virtual DbSet<TbOrderStatus> TbOrderStatuses { get; set; }

    public virtual DbSet<TbProduct> TbProducts { get; set; }

    public virtual DbSet<TbProductReview> TbProductReviews { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HJV1A73;Initial Catalog=LaptopShop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("TbAccount");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Avatar).HasMaxLength(550);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.TbAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_TbAccount_TbRole");
        });

        modelBuilder.Entity<TbBrand>(entity =>
        {
            entity.HasKey(e => e.BrandId);

            entity.ToTable("TbBrand");

            entity.Property(e => e.Banner).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TbCategoryNew>(entity =>
        {
            entity.HasKey(e => e.CategoryNewId);

            entity.ToTable("TbCategoryNew");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TbCategoryProduct>(entity =>
        {
            entity.HasKey(e => e.CategoryProductId);

            entity.ToTable("TbCategoryProduct");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TbContact>(entity =>
        {
            entity.HasKey(e => e.ContactId);

            entity.ToTable("TbContact");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Message).HasMaxLength(550);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<TbCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("TbCustomer");

            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TbImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("TbImage");

            entity.Property(e => e.ImagePath).HasMaxLength(250);

            entity.HasOne(d => d.Product).WithMany(p => p.TbImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbImage_TbProduct");
        });

        modelBuilder.Entity<TbMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("TbMenu");

            entity.Property(e => e.ActionName).HasMaxLength(50);
            entity.Property(e => e.ControllerName).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbMenuAdmin>(entity =>
        {
            entity.HasKey(e => e.MenuAdminId);

            entity.ToTable("TbMenuAdmin");

            entity.Property(e => e.ActionName).HasMaxLength(250);
            entity.Property(e => e.AreaName).HasMaxLength(250);
            entity.Property(e => e.ControllerName).HasMaxLength(250);
            entity.Property(e => e.Icon).HasMaxLength(250);
            entity.Property(e => e.ItemName).HasMaxLength(250);
        });

        modelBuilder.Entity<TbNews>(entity =>
        {
            entity.HasKey(e => e.NewId);

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(550);
            entity.Property(e => e.Image).HasMaxLength(550);
            entity.Property(e => e.Tags).HasMaxLength(550);
            entity.Property(e => e.Title).HasMaxLength(550);

            entity.HasOne(d => d.CategoryNew).WithMany(p => p.TbNews)
                .HasForeignKey(d => d.CategoryNewId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbNews_TbCategoryNew");
        });

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("TbOrder");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 5)");

            entity.HasOne(d => d.Customer).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbOrder_TbCustomer");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK_TbOrder_TbOrderStatus");
        });

        modelBuilder.Entity<TbOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId);

            entity.ToTable("TbOrderDetail");


            entity.HasOne(d => d.Order).WithMany(p => p.TbOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_TbOrderDetail_TbOrder");

            entity.HasOne(d => d.Product).WithMany(p => p.TbOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbOrderDetail_TbProduct");
        });

        modelBuilder.Entity<TbOrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId);

            entity.ToTable("TbOrderStatus");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TbProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("TbProduct");

            entity.Property(e => e.Bluetooth).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CardBrand).HasMaxLength(50);
            entity.Property(e => e.CardModel).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.ConnectPort).HasMaxLength(250);
            entity.Property(e => e.CpuCompany).HasMaxLength(50);
            entity.Property(e => e.CpuMaxSpeed).HasMaxLength(50);
            entity.Property(e => e.CpuSpeed).HasMaxLength(50);
            entity.Property(e => e.CpuType).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(550);
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 5)");
            entity.Property(e => e.DriveMemory).HasMaxLength(50);
            entity.Property(e => e.DriveType).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(550);
            entity.Property(e => e.Material).HasMaxLength(250);
            entity.Property(e => e.Origin).HasMaxLength(50);
            entity.Property(e => e.Os).HasMaxLength(50);
            entity.Property(e => e.PinCapacity).HasMaxLength(50);
            entity.Property(e => e.PinType).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductName).HasMaxLength(550);
            entity.Property(e => e.RamSize).HasMaxLength(50);
            entity.Property(e => e.RamSpeed).HasMaxLength(50);
            entity.Property(e => e.RamSupportMax).HasMaxLength(50);
            entity.Property(e => e.RamType).HasMaxLength(50);
            entity.Property(e => e.ScreenPanel).HasMaxLength(50);
            entity.Property(e => e.ScreenPixel).HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.Size).HasMaxLength(250);
            entity.Property(e => e.Version).HasMaxLength(50);
            entity.Property(e => e.Webcam).HasMaxLength(250);
            entity.Property(e => e.Weight).HasMaxLength(50);
            entity.Property(e => e.Wifi).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.TbProducts)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbProduct_TbBrand");

            entity.HasOne(d => d.CategoryProduct).WithMany(p => p.TbProducts)
                .HasForeignKey(d => d.CategoryProductId)
                .HasConstraintName("FK_TbProduct_TbCategoryProduct");
        });

        modelBuilder.Entity<TbProductReview>(entity =>
        {
            entity.HasKey(e => e.ProductReviewId);

            entity.ToTable("TbProductReview");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Message).HasMaxLength(550);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Product).WithMany(p => p.TbProductReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbProductReview_TbProduct");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("TbRole");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
