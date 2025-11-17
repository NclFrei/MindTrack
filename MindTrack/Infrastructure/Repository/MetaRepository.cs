using Microsoft.EntityFrameworkCore;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Models;
using MindTrack.Infrastructure.Data;

namespace MindTrack.Infrastructure.Repository;

public class MetaRepository : IMetaRepository
{
    private readonly MindTrackContext _context;

    public MetaRepository(MindTrackContext context)
    {
        _context = context;
    }

    public async Task<Meta> CreateAsync(Meta meta)
    {
        _context.Metas.Add(meta);
        await _context.SaveChangesAsync();
        return meta;
    }

    public async Task<List<Meta>> GetAllAsync()
    {
        return await _context.Metas
            .ToListAsync();
    }

    public async Task<Meta?> GetByIdAsync(int id)
    {
        return await _context.Metas.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Meta?> UpdateAsync(Meta area)
    {
        _context.Metas.Update(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var area = await _context.Metas.FindAsync(id);
        if (area == null) return false;

        _context.Metas.Remove(area);
        await _context.SaveChangesAsync();
        return true;
    }
}
