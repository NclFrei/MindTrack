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
        public DbSet<Meta> Metas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.Metas)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meta>()
                .Property(m => m.Concluida)
                .HasConversion(
                    v => v ? 1 : 0,  // C# → Oracle
                    v => v == 1      // Oracle → C#
    );

            base.OnModelCreating(modelBuilder);
        }

    }
}
