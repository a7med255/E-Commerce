using System;
using System.Collections.Generic;
using E_Commerce.Data;
using E_Commerce.Bl;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models;

public partial class ECommerceContext : IdentityDbContext<ApplicationUser>
{

    public ECommerceContext()
    {
    }

    public ECommerceContext(DbContextOptions<ECommerceContext> options)
        : base(options)
    {
    }


    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }


    public virtual DbSet<TbBusinessInfo> TbBusinessInfos { get; set; }

    public virtual DbSet<TbCashTransacion> TbCashTransacions { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbCustomer> TbCustomers { get; set; }

    public virtual DbSet<TbItem> TbItems { get; set; }

    public virtual DbSet<TbItemDiscount> TbItemDiscounts { get; set; }

    public virtual DbSet<TbItemType> TbItemTypes { get; set; }

    public virtual DbSet<TbPage> TbPages { get; set; }

    public virtual DbSet<TbPurchaseInvoice> TbPurchaseInvoices { get; set; }

    public virtual DbSet<TbPurchaseInvoiceItem> TbPurchaseInvoiceItems { get; set; }

    public virtual DbSet<TbSalesInvoice> TbSalesInvoices { get; set; }

    public virtual DbSet<TbSalesInvoiceItem> TbSalesInvoiceItems { get; set; }

    public virtual DbSet<TbSetting> TbSettings { get; set; }

    public virtual DbSet<TbSlider> TbSliders { get; set; }

    public virtual DbSet<TbSupplier> TbSuppliers { get; set; }

    public virtual DbSet<TbWishlistItem> TbWishlistItems { get; set; }

    public virtual DbSet<VwItem> VwItems { get; set; }

    public virtual DbSet<VwItemCategory> VwItemCategories { get; set; }

    public virtual DbSet<VwItemsOutOfInvoice> VwItemsOutOfInvoices { get; set; }

    public virtual DbSet<VwSalesInvoice> VwSalesInvoices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<TbBusinessInfo>(entity =>
        {
            entity.HasKey(e => e.BusinessInfoId);

            entity.ToTable("TbBusinessInfo");

            entity.HasIndex(e => e.CutomerId, "IX_TbBusinessInfo_CutomerId").IsUnique();

            entity.Property(e => e.Budget).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.BusinessCardNumber).HasMaxLength(20);

            entity.HasOne(d => d.Cutomer).WithOne(p => p.TbBusinessInfo).HasForeignKey<TbBusinessInfo>(d => d.CutomerId);
        });

        modelBuilder.Entity<TbCashTransacion>(entity =>
        {
            entity.HasKey(e => e.CashTransactionId);

            entity.ToTable("TbCashTransacion");

            entity.Property(e => e.CashDate).HasColumnType("datetime");
            entity.Property(e => e.CashValue).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasDefaultValue("");
            entity.Property(e => e.ImageName).HasDefaultValue("");
        });

        modelBuilder.Entity<TbCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.HasIndex(e => e.ItemTypeId, "IX_TbItems_ItemTypeId");

            entity.Property(e => e.CreatedBy).HasDefaultValue("");
            entity.Property(e => e.CreatedDate).HasDefaultValue(new DateTime(2020, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));
            entity.Property(e => e.ImageName).HasMaxLength(200);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.ItemTypeId).HasDefaultValue(0);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.SalesPrice).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.TbItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbItems_TbCategories");

            entity.HasOne(d => d.ItemType).WithMany(p => p.TbItems)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_TbItems_TbItemTypes");

            entity.HasMany(d => d.Customers).WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "TbCustomerItem",
                    r => r.HasOne<TbCustomer>().WithMany().HasForeignKey("CustomerId"),
                    l => l.HasOne<TbItem>().WithMany().HasForeignKey("ItemId"),
                    j =>
                    {
                        j.HasKey("ItemId", "CustomerId");
                        j.ToTable("TbCustomerItems");
                        j.HasIndex(new[] { "CustomerId" }, "IX_TbCustomerItems_CustomerId");
                    });
        });

        modelBuilder.Entity<CartItem>()
        .HasOne(c => c.Item) // CartItem has one Product
        .WithMany(p => p.CartItems) // Product has many CartItems
        .HasForeignKey(c => c.ProductId);

        modelBuilder.Entity<Order>()
       .HasMany(o => o.OrderItems)
       .WithOne(oi => oi.Order)
       .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Item)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<TbItemDiscount>(entity =>
        {
            entity.HasKey(e => e.ItemDiscountId);

            entity.ToTable("TbItemDiscount");

            entity.HasIndex(e => e.ItemId, "IX_TbItemDiscount_ItemId");

            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.TbItemDiscounts)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbItemDiscounts_TbItems");
        });

        modelBuilder.Entity<TbItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId);

            entity.Property(e => e.ItemTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbPage>(entity =>
        {
            entity.HasKey(e => e.PageId);

            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<TbPurchaseInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Supplier).WithMany(p => p.TbPurchaseInvoices)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbPurchaseInvoices_TbSuppliers");
        });

        modelBuilder.Entity<TbPurchaseInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemId);

            entity.Property(e => e.InvoiceItemId).ValueGeneratedNever();
            entity.Property(e => e.InvoicePrice).HasColumnType("decimal(8, 4)");
            entity.Property(e => e.Qty).HasDefaultValue(1.0);

            entity.HasOne(d => d.Invoice).WithMany(p => p.TbPurchaseInvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbPurchaseInvoiceItems_TbPurchaseInvoices");

            entity.HasOne(d => d.Item).WithMany(p => p.TbPurchaseInvoiceItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbPurchaseInvoiceItems_TbItems");
        });

        modelBuilder.Entity<TbSalesInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.Property(e => e.CreatedBy).HasDefaultValue("");
            entity.Property(e => e.DelivryDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TbSalesInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemId);

            entity.Property(e => e.InvoicePrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Qty).HasDefaultValue(1.0);
            entity.Property(e => e.Size)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Invoice).WithMany(p => p.TbSalesInvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbSalesInvoiceItems_TbSalesInvoices");

            entity.HasOne(d => d.Item).WithMany(p => p.TbSalesInvoiceItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TbSalesInvoiceItems_TbItems");
        });

        modelBuilder.Entity<TbSlider>(entity =>
        {
            entity.HasKey(e => e.SliderId);

            entity.ToTable("TbSlider");

            entity.Property(e => e.CreatedBy).HasDefaultValue("");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageName).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<TbSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK_TbSupplier");

            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwItems");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.ImageName).HasMaxLength(200);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.ItemTypeName).HasMaxLength(100);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.SalesPrice).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<VwItemCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwItemCategories");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.ImageName).HasMaxLength(200);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.SalesPrice).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<VwItemsOutOfInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwItemsOutOfInvoices");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.InvoicePrice).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<VwSalesInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwSalesInvoices");

            entity.Property(e => e.DelivryDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
