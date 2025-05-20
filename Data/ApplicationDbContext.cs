
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniprojet.Models;

namespace miniprojet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Appartement> Appartements { get; set; }
        public DbSet<Propriétaire> Propriétaires { get; set; }
        public DbSet<Locataire> Locataires { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Compte
            modelBuilder.Entity<Compte>()
                .Property(c => c.Role)
                .HasDefaultValue("User");

            // Appartement
            modelBuilder.Entity<Appartement>()
                .HasOne(a => a.Propriétaire)
                .WithMany(p => p.Appartements)
                .HasForeignKey(a => a.IdProp)
                .OnDelete(DeleteBehavior.Cascade);

            // Location
            modelBuilder.Entity<Location>()
                .HasOne(l => l.Locataire)
                .WithMany(loc => loc.Locations)
                .HasForeignKey(l => l.IdLoc)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Appartement)
                .WithMany()
                .HasForeignKey(l => l.NumApp)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraints
            modelBuilder.Entity<Compte>()
                .HasIndex(c => c.Username)
                .IsUnique();
        }
    }
}
