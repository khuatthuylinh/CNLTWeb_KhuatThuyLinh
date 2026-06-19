using Microsoft.EntityFrameworkCore;

namespace MID_BIT247620.Models
{
    public class RoomDbContext : DbContext
    {
        public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options)
        {
        }

        public DbSet<RoomType_BIT247620> RoomTypes_BIT247620 { get; set; }
        public DbSet<Room_BIT247620> Rooms_BIT247620 { get; set; }
        public DbSet<RoomImage_BIT247620> RoomImages_BIT247620 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names
            modelBuilder.Entity<RoomType_BIT247620>().ToTable("RoomTypes_BIT247620");
            modelBuilder.Entity<Room_BIT247620>().ToTable("Rooms_BIT247620");
            modelBuilder.Entity<RoomImage_BIT247620>().ToTable("RoomImages_BIT247620");

            // Configure RoomType -> Room relationship (1-N)
            modelBuilder.Entity<Room_BIT247620>()
                .HasOne(r => r.RoomType)
                .WithMany(t => t.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting type if rooms exist

            // Configure Room -> RoomImage relationship (1-N)
            modelBuilder.Entity<RoomImage_BIT247620>()
                .HasOne(i => i.Room)
                .WithMany(r => r.RoomImages)
                .HasForeignKey(i => i.RoomId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete images when room is deleted
        }
    }
}
