using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Warehouse_API_Test.Models;

public partial class WarehouseDbContext : DbContext
{
    public WarehouseDbContext()
    {
    }

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=warehouse;Username=postgres;Password=Pinguindercoole");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Box_pkey");

            entity.ToTable("Box");

            entity.Property(e => e.Qrcode).HasColumnName("QRcode");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sets_pkey");

            entity.HasIndex(e => e.BoxId, "IX_Sets_BoxID");

            entity.Property(e => e.AgeRangeId).HasColumnName("AgeRange_Id");
            entity.Property(e => e.AgeRangeMax).HasColumnName("AgeRange_Max");
            entity.Property(e => e.AgeRangeMin).HasColumnName("AgeRange_Min");
            entity.Property(e => e.BarcodeEan).HasColumnName("Barcode_EAN");
            entity.Property(e => e.BarcodeUpc).HasColumnName("Barcode_UPC");
            entity.Property(e => e.BoxId).HasColumnName("BoxID");
            entity.Property(e => e.DimensionsDepth).HasColumnName("Dimensions_Depth");
            entity.Property(e => e.DimensionsHeight).HasColumnName("Dimensions_Height");
            entity.Property(e => e.DimensionsWeight).HasColumnName("Dimensions_Weight");
            entity.Property(e => e.DimensionsWidth).HasColumnName("Dimensions_Width");
            entity.Property(e => e.ExtendedDataBrickTags).HasColumnName("ExtendedData_BrickTags");
            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageImageUrl).HasColumnName("Image_ImageURL");
            entity.Property(e => e.ImageThumbnailUrl).HasColumnName("Image_ThumbnailURL");
            entity.Property(e => e.LegocomCaDateFirstAvailable).HasColumnName("LEGOCom_CA_DateFirstAvailable");
            entity.Property(e => e.LegocomCaDateLastAvailable).HasColumnName("LEGOCom_CA_DateLastAvailable");
            entity.Property(e => e.LegocomCaRetailPrice).HasColumnName("LEGOCom_CA_RetailPrice");
            entity.Property(e => e.LegocomDeDateFirstAvailable).HasColumnName("LEGOCom_DE_DateFirstAvailable");
            entity.Property(e => e.LegocomDeDateLastAvailable).HasColumnName("LEGOCom_DE_DateLastAvailable");
            entity.Property(e => e.LegocomDeRetailPrice).HasColumnName("LEGOCom_DE_RetailPrice");
            entity.Property(e => e.LegocomId).HasColumnName("LEGOCom_Id");
            entity.Property(e => e.LegocomUkDateFirstAvailable).HasColumnName("LEGOCom_UK_DateFirstAvailable");
            entity.Property(e => e.LegocomUkDateLastAvailable).HasColumnName("LEGOCom_UK_DateLastAvailable");
            entity.Property(e => e.LegocomUkRetailPrice).HasColumnName("LEGOCom_UK_RetailPrice");
            entity.Property(e => e.LegocomUsDateFirstAvailable).HasColumnName("LEGOCom_US_DateFirstAvailable");
            entity.Property(e => e.LegocomUsDateLastAvailable).HasColumnName("LEGOCom_US_DateLastAvailable");
            entity.Property(e => e.LegocomUsRetailPrice).HasColumnName("LEGOCom_US_RetailPrice");

            entity.HasOne(d => d.Box).WithMany(p => p.Sets).HasForeignKey(d => d.BoxId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
