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


    }
}
