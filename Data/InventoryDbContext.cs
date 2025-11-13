using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;

public class InventoryDbContext : DbContext
{
    public DbSet<Printer> printers;
    public DbSet<Brand> brands;
    public DbSet<Toner> toners;

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {  }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserTonerRequest>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserTonerRequest>()
            .HasMany(r => r.TonerRequests)
            .WithOne(t => t.UserTonerRequest)
            .HasForeignKey(t => t.UserTonerRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}