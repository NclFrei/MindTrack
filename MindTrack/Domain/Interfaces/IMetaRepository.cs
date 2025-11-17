using MindTrack.Domain.Models;

namespace MindTrack.Domain.Interfaces;

public interface IMetaRepository
{
    Task<Meta> CreateAsync(Meta area);
    Task<List<Meta>> GetAllAsync();
    Task<Meta?> GetByIdAsync(int id);
    Task<Meta?> UpdateAsync(Meta area);
    Task<bool> DeleteAsync(int id);
}
