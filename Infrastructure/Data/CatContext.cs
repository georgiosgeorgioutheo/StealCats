using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class CatContext : DbContext
    {
        public CatContext(DbContextOptions<CatContext> options) : base(options) { }

        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<CatTagEntity> CatTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatEntity>()
               .Property(c => c.Id)
               .ValueGeneratedOnAdd(); // Ensure Id is auto-incremented

            modelBuilder.Entity<CatTagEntity>()
                .HasKey(ct => new { ct.CatId, ct.TagId });

            modelBuilder.Entity<CatTagEntity>()
                .HasOne(ct => ct.Cat)
                .WithMany(c => c.CatTags)
                .HasForeignKey(ct => ct.CatId);

            modelBuilder.Entity<CatTagEntity>()
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.CatTags)
                .HasForeignKey(ct => ct.TagId);

          
        }
    }
}
