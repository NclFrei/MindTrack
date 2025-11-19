using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Data;

namespace MindTrack.Infrastructure.Repository;

public class HeartMetricRepository : IHeartMetricRepository
{
    private readonly MindTrackContext _context;
    public HeartMetricRepository(MindTrackContext context) { _context = context; }

    public async Task<HeartMetric> CreateAsync(HeartMetric metric)
    {
        _context.Add(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task<List<HeartMetric>> GetByUserAsync(int userId, DateTime? from = null, DateTime? to = null)
    {
        var q = _context.Set<HeartMetric>().AsQueryable().Where(m => m.UserId == userId);
        if (from.HasValue) q = q.Where(m => m.Timestamp >= from.Value);
        if (to.HasValue) q = q.Where(m => m.Timestamp <= to.Value);
        return await q.ToListAsync();
    }

    public async Task<HeartMetric?> GetByIdAsync(int id)
    {
        return await _context.Set<HeartMetric>().FirstOrDefaultAsync(m => m.Id == id);
    }
}
