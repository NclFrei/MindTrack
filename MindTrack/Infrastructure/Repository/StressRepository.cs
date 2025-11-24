using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Data;

namespace MindTrack.Infrastructure.Repository;

public class StressRepository : IStressRepository
{
    private readonly MindTrackContext _context;
    public StressRepository(MindTrackContext context) { _context = context; }

    public async Task<StressScore> CreateAsync(StressScore score)
    {
        _context.Add(score);
        await _context.SaveChangesAsync();
        return score;
    }

    public async Task<List<StressScore>> GetByUserAsync(int userId, DateTime? from = null, DateTime? to = null)
    {
        var q = _context.Set<StressScore>().AsQueryable().Where(s => s.UserId == userId);
        if (from.HasValue) q = q.Where(s => s.Timestamp >= from.Value);
        if (to.HasValue) q = q.Where(s => s.Timestamp <= to.Value);
        return await q.ToListAsync();
    }

    public async Task<StressScore?> GetByIdAsync(int id)
    {
        return await _context.Set<StressScore>().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<StressScore?> GetLatestByUserIdAsync(int userId)
    {
        return await _context.StressScores
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Timestamp) 
            .FirstOrDefaultAsync();
    }
}
