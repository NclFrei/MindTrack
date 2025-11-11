using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Models;
using System.IO;

namespace MindTrack.Infrastructure.Data
{
    public class MindTrackContext : DbContext
    {
        public MindTrackContext(DbContextOptions<MindTrackContext> options) : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Meta> Meta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.Metas)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
