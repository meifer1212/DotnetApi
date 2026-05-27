using DotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetApi.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("TaskItems");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.IsCompleted)
                    .HasDefaultValue(false);

                entity.HasOne(t => t.User)
                   .WithMany(u => u.TaskItems)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
