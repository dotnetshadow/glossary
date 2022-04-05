using Glossary.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Glossary.Api.Data
{
    public class GlossaryDbContext : DbContext
    {
        public GlossaryDbContext(DbContextOptions<GlossaryDbContext> options) : base(options)
        {
        }

        public DbSet<GlossaryItem> GlossaryItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Id
            modelBuilder.Entity<GlossaryItem>()
                .HasKey(g => g.Id);


            // Term
            modelBuilder.Entity<GlossaryItem>()
                .Property(g => g.Term)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<GlossaryItem>()
                .HasIndex(g => g.Term).IsUnique();

            // Definition
            modelBuilder.Entity<GlossaryItem>()
                .Property(g => g.Definition)
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}