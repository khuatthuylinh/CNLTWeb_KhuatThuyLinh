using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Author).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
}
