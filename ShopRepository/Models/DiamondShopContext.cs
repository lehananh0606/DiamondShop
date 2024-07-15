using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ShopRepository.Models;

public partial class DiamondShopContext : DbContext
{
    public DiamondShopContext()
    {
    }

    public DiamondShopContext(DbContextOptions<DiamondShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auction> Auctions { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Chỉ cấu hình nếu options chưa được cấu hình
        if (!optionsBuilder.IsConfigured)
        {
            // Lấy chuỗi kết nối từ IConfiguration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Sử dụng chuỗi kết nối đã lấy được
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasIndex(e => e.AuctionId, "IX_Bids_AuctionID");

            entity.HasIndex(e => e.UserId, "IX_Bids_UserID");

            entity.Property(e => e.BidId).HasColumnName("BidID");
            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Auction).WithMany(p => p.Bids)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User).WithMany(p => p.Bids)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Notifications_UserID");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.AuctionId, "IX_Orders_AuctionID").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_Orders_UserID");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Auction).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasIndex(e => e.AuctionId, "IX_ProductImages_AuctionID");

            entity.Property(e => e.ProductImageId).HasColumnName("ProductImageID");
            entity.Property(e => e.AuctionId).HasColumnName("AuctionID");

            entity.HasOne(d => d.Auction).WithMany(p => p.ProductImages).HasForeignKey(d => d.AuctionId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_Transactions_OrderID");

            entity.HasIndex(e => e.WalletId, "IX_Transactions_WalletID");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.Order).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions).HasForeignKey(d => d.WalletId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Wallets_UserID").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ_Wallets_UserID").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Wallet).HasForeignKey<Wallet>(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
