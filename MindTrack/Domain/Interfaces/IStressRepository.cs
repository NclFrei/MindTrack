using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces;

public interface IStressRepository
{
    Task<StressScore> CreateAsync(StressScore score);
    Task<List<StressScore>> GetByUserAsync(int userId, DateTime? from = null, DateTime? to = null);
    Task<StressScore?> GetByIdAsync(int id);
    Task<StressScore?> GetLatestByUserIdAsync(int userId);

}
