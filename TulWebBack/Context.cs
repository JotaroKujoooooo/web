using Microsoft.EntityFrameworkCore;
using TulWebBack.Entities;

namespace TulWebBack
{
    public class Context : DbContext
    {
        public Context(DbContextOptions dbContext) : base(dbContext)
        {
            Database.EnsureCreated();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemProperties> ItemProperties { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Item>()
                .HasOne(pi => pi.Props)
                .WithOne(pr => pr.Item)
                .HasForeignKey<ItemProperties>(pr => pr.ItemId);

            modelBuilder
                .Entity<Item>()
                .HasOne(pi => pi.Image)
                .WithOne(pr => pr.Item)
                .HasForeignKey<ItemImage>(pr => pr.ItemId);

            modelBuilder
               .Entity<Item>()
               .HasMany(pr => pr.Customers)
               .WithMany(u => u.Items);
        }
    }
}
