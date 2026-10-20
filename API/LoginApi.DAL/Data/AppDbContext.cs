
using LoginApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginApi.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "dbo");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .HasColumnName("Name")
                .IsRequired();

            entity.Property(u => u.Email)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
        });
    }
}
