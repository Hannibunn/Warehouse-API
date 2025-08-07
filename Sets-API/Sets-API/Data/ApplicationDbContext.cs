using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Sets_API.Models;

namespace Sets_API.Data
{
    public partial class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
        public DbSet<Set> Sets { get; set; }
        public DbSet<Box> Boxes { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=setsdb;Username=postgres;Password=Lion");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Set>()
                .HasIndex(s => s.number);

            modelBuilder.Entity<Set>()
                .HasIndex(s => s.name);

            modelBuilder.Entity<Set>()
                .OwnsOne(s => s.Image);

            modelBuilder.Entity<Set>()
               .OwnsOne(s => s.Barcode);

            modelBuilder.Entity<Set>()
               .OwnsOne(s => s.Dimensions);

            modelBuilder.Entity<Set>()
              .OwnsOne(s => s.AgeRange);

            modelBuilder.Entity<Set>()
               .OwnsOne(s => s.ExtendedData);

            modelBuilder.Entity<Set>()
               .OwnsOne(s => s.LEGOCom);

            modelBuilder.Entity<Set>()
               .OwnsOne(s => s.LEGOCom, legoCom =>
               {
                   // Configure owned entities
                   legoCom.OwnsOne(lc => lc.US);
                   legoCom.OwnsOne(lc => lc.UK);
                   legoCom.OwnsOne(lc => lc.CA);
                   legoCom.OwnsOne(lc => lc.DE);
               });
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Number)
            //    .IsRequired()
            //    .HasMaxLength(20);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Name)
            //    .IsRequired()
            //    .HasMaxLength(200);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Theme)
            //    .HasMaxLength(100);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.ThemeGroup)
            //    .HasMaxLength(100);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.SubTheme)
            //    .HasMaxLength(100);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Category)
            //    .HasMaxLength(100);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.PackagingType)
            //    .HasMaxLength(50);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Category)
            //    .HasMaxLength(100);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Availability)
            //    .HasMaxLength(50);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.InstructionsCount)
            //    .HasDefaultValue(0);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Year)
            //    .HasDefaultValue(DateTime.Now.Year);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Released)
            //    .HasDefaultValue(true);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Pieces)
            //    .HasDefaultValue(0);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.Minifigs)
            //    .HasDefaultValue(0);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.NumberVariant)
            //    .HasDefaultValue(0);
            //modelBuilder.Entity<Set>()
            //    .Property(s => s.SetID)
            //    .ValueGeneratedOnAddOrUpdate()
            //    .HasDefaultValue(null);
            //modelBuilder.Entity<Set>()
            //    .HasOne(s => s.Box)
            //    .WithMany(b => b.Sets)
            //    .HasForeignKey(s => s.BoxID)
            //    .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<Set>()
            //    .HasKey(s => s.ID);

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
