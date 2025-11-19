using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces;

public interface IHeartMetricRepository
{
    Task<HeartMetric> CreateAsync(HeartMetric metric);
    Task<List<HeartMetric>> GetByUserAsync(int userId, DateTime? from = null, DateTime? to = null);
    Task<HeartMetric?> GetByIdAsync(int id);
}
