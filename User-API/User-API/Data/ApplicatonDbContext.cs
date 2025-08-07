using Microsoft.EntityFrameworkCore;
using User_API.Models;

namespace User_API.Data
{
    public class ApplicatonDbContext: DbContext
    {
        public ApplicatonDbContext(DbContextOptions<ApplicatonDbContext> options)
          : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Set> Sets { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=setsdb;Username=postgres;Password=Lion");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Indizes für Set
            modelBuilder.Entity<Set>()
                .HasIndex(s => s.number);
            modelBuilder.Entity<Set>()
                .HasIndex(s => s.name);

            // Owned Entities
            modelBuilder.Entity<Set>().OwnsOne(s => s.Image);
            modelBuilder.Entity<Set>().OwnsOne(s => s.AgeRange);
            modelBuilder.Entity<Set>().OwnsOne(s => s.Dimensions);
            modelBuilder.Entity<Set>().OwnsOne(s => s.ExtendedData);
            modelBuilder.Entity<Set>().OwnsOne(s => s.LEGOCom, legoCom =>
            {
                legoCom.OwnsOne(lc => lc.US);
                legoCom.OwnsOne(lc => lc.UK);
                legoCom.OwnsOne(lc => lc.CA);
                legoCom.OwnsOne(lc => lc.DE);
            });

            // Set -> Box (optional)
            modelBuilder.Entity<Set>()
                .HasOne(s => s.box)
                .WithMany(b => b.Sets)
                .HasForeignKey(s => s.boxID)
                .OnDelete(DeleteBehavior.SetNull);

            // Box -> User
            modelBuilder.Entity<Box>()
                .HasOne(b => b.User)
                .WithMany(u => u.Boxes)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Box -> Storage (optional)
            modelBuilder.Entity<Box>()
                .HasOne(b => b.Storage)
                .WithMany(s => s.Boxes)
                .HasForeignKey(b => b.StorageID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
